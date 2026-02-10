import axios from 'axios';
import { tokenStorage } from '../storage/tokenStorage';

export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5243/api',
});

api.interceptors.request.use((config) => {
  const token = tokenStorage.get();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      tokenStorage.remove();
      if (typeof window !== 'undefined') {
        window.location.href = '/login';
      }
    }

    const data = error.response?.data;
    if (data) {
      const message = data.errors?.length
        ? data.errors.join('. ')
        : data.message || error.message;
      return Promise.reject(new Error(message));
    }

    return Promise.reject(error);
  }
);
