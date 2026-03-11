import axios from "axios";

const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_BASE_URL || "http://localhost:5248/api",
  timeout: 5000,
  headers: {
    "Content-Type": "application/json",
  },
});

// Module-level callback — set by AuthProvider so the interceptor can call logout()
let onUnauthorized: (() => void) | null = null;

export function setUnauthorizedHandler(handler: () => void): void {
  onUnauthorized = handler;
}

// Request interceptor — attaches Bearer token from localStorage
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

// Response interceptor — unwraps .data; calls logout() from AuthContext on 401
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
      // Delegate to AuthContext logout — it clears state and the AuthGuard handles redirect
      if (onUnauthorized) {
        onUnauthorized();
      } else {
        // Fallback: clear storage directly if context is not yet mounted
        localStorage.removeItem("token");
        localStorage.removeItem("role");
        localStorage.removeItem("username");
      }
    }

    console.error("API Error:", error.message);
    return Promise.reject(error);
  }
);

export default apiClient;