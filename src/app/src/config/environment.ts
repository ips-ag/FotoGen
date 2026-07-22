// Environment configuration
const isDevelopment = import.meta.env.DEV;
const isProduction = import.meta.env.PROD;

export const config = {
  API_ROOT: import.meta.env.VITE_API_ROOT,
  // Other environment-specific configs can go here
  isDevelopment,
  isProduction,
};
