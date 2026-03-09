import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 5000,
  headers: {
    "Content-Type": "application/json",
  },
});

// Request interceptor
apiClient.interceptors.request.use(
  (config) => {
    console.log(
      `Sending ${config.method?.toUpperCase()} request to ${config.url} with data:`,
      config.data
    );
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    console.error("Error in request interceptor:", error);
    return Promise.reject(error);
  }
);

// Response interceptor
apiClient.interceptors.response.use(
  (response) => response.data,
  (error) => {
    // Ignore cancellation errors
    if (axios.isCancel(error) || error?.code === "ERR_CANCELED") {
      console.log("Request cancelled by AbortController");
      return Promise.reject(error);
    }

    // Handle unauthenticated requests globally
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      window.location.href = "/";
    }

    console.error("API Error:", error.message);
    return Promise.reject(error);
  }
);

export default apiClient;