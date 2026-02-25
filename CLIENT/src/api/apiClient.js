import axios from 'axios';

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
    },
})

//request interceptor
apiClient.interceptors.request.use(
    (config) => {

        console.log(`Sending ${config.method?.toUpperCase()} request to ${config.url} with data: `, config.data);
        const token = localStorage.getItem('token');
        // If token exists, add it to the Authorization header
        if (token)
            config.headers.Authorization = `Bearer ${token}`;
        return config;
    }, 
    // Handle request errors when they occur because of network issues or other reasons
    (error) => {
        console.error('Error in request interceptor: ', error);
        return Promise.reject(error);
    }
);

//Response interceptor
// This will handle the response and extract the data, and also catch any errors that occur during the response phase, such as server errors or invalid responses.
apiClient.interceptors.response.use(
  (response) => response.data,
  (error) => {
    console.error("API Error:", error.message);
    return Promise.reject(error);
  }
);

export default apiClient;