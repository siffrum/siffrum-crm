import axios from "axios";
import { environment } from "src/environments/environment";

export const apiClient = axios.create({
  baseURL: environment.apiBaseUrl,
  timeout: environment.apiDefaultTimeout * 1000,
  headers: {
    "ngrok-skip-browser-warning": "true",
    "Content-Type": "application/json",
  },
});

// Optional: helpful debug + safety
apiClient.interceptors.response.use(
  (res) => res,
  (error) => {
    // if ngrok returns HTML, error.response.data might be a string
    const data = error?.response?.data;
    if (typeof data === "string" && data.includes("<!DOCTYPE html") && data.includes("ngrok")) {
      console.error("NGROK HTML WARNING PAGE received. Add ngrok-skip-browser-warning header.");
    }
    return Promise.reject(error);
  }
);
