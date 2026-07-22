
import { useCallback } from 'react';
import { useMsal } from '@azure/msal-react';
import { config } from '@/config/environment';

const API_ROOT = config.API_ROOT;

export interface GeneratePhotoRequest {
  ModelName: string | null;
  Prompt: string;
}

export interface GeneratePhotoResponse {
  imageUrl: string;
  success: boolean;
  message?: string;
  errorCode?: string;
}

export interface CheckUserModelResponse {
  hasModel: boolean;
  modelName?: string;
  success: boolean;
  message?: string;
}

export interface UploadZipResponse {
  url: string;
  success: boolean;
  message?: string;
}

export interface TrainModelRequest {
  imageUrl: string;
}

export interface TrainModelResponse {
  success: boolean;
  message?: string;
  modelId?: string;
  errorCode?: string;
}

export const useApi = () => {
  const { instance } = useMsal();

  // Helper function to get auth token from MSAL
  const getAuthToken = useCallback(async (): Promise<string | null> => {
    try {
      if (!instance) {
        console.error('MSAL instance not available');
        return null;
      }

      const accounts = instance.getAllAccounts();
      if (accounts.length === 0) {
        console.error('No accounts found in MSAL');
        return null;
      }

      const account = accounts[0];
      const tokenRequest = {
        scopes: ["openid", "profile", "email", `${import.meta.env.VITE_AZURE_CLIENT_ID}/FotoGen`],
        account: account
      };

      const response = await instance.acquireTokenSilent(tokenRequest);
      console.log('Token acquired silently from MSAL');
      return response.accessToken;
    } catch (error) {
      console.error('Error acquiring token from MSAL:', error);
      return null;
    }
  }, [instance]);

  // 1. API to check if user has model or not
  const checkUserModelAvailable = useCallback(async (userId?: string, modelName?: string | null): Promise<CheckUserModelResponse> => {
    try {
      console.log('useApi.checkUserModelAvailable called with:');
      console.log('  - userId:', userId);
      console.log('  - modelName:', modelName);
      console.log('  - modelName type:', typeof modelName);
      
      if (!userId) {
        console.log('useApi - No userId provided, returning error');
        return {
          hasModel: false,
          success: false,
          message: 'User ID is required'
        };
      }
      
      const token = await getAuthToken();
      const headers: HeadersInit = {
        'Content-Type': 'application/json',
      };
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
      
      // Build URL - only add modelName parameter if it's not null
      let fullUrl = `${API_ROOT}/integration/check-user-model-available`;
      if (modelName !== null && modelName !== undefined) {
        fullUrl += `?modelName=${modelName}`;
        console.log('useApi - Added modelName to URL:', modelName);
      } else {
        console.log('useApi - modelName is null/undefined, not adding to URL');
      }
      
      console.log('useApi - Full API URL:', fullUrl);
      
      const response = await fetch(fullUrl, {
        method: 'GET',
        headers: headers
      });
      
      console.log('useApi - Response status:', response.status);
      
      if (response.status === 200) {
        const data = await response.json();
        console.log('useApi - Response data (200):', data);
        return {
          hasModel: true,
          modelName: modelName || userId,
          success: true,
          message: 'User model found successfully'
        };
      } else if (response.status === 404) {
        const data = await response.json();
        console.log('useApi - Response data (404):', data);
        return {
          hasModel: false,
          success: false,
          message: data.message || 'Model not found'
        };
      } else {
        console.log('useApi - Unexpected response status:', response.status);
        return {
          hasModel: false,
          success: false,
          message: 'Failed to check user model availability'
        };
      }
    } catch (error) {
      console.error('useApi - Error checking user model:', error);
      return {
        hasModel: false,
        success: false,
        message: 'Failed to check user model availability'
      };
    }
  }, [getAuthToken]);

  // 2. Generate photo from model
  const generatePhoto = useCallback(async (request: GeneratePhotoRequest): Promise<GeneratePhotoResponse> => {
    try {
      console.log('Generating photo with request:', request);
      
      const token = await getAuthToken();
      const headers: HeadersInit = {
        'Content-Type': 'application/json',
      };
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
      
      // Set ModelName to null if using user's own model
      const requestBody = {
        ModelName: request.ModelName === 'user-model' ? null : request.ModelName,
        Prompt: request.Prompt
      };
      
      const response = await fetch(`${API_ROOT}/integration/generate-photo`, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(requestBody)
      });
      
      // Always try to parse the response body, regardless of status
      const data = await response.json();
      console.log('API response data:', data);
      
      if (response.ok && data.isSuccess) {
        // Convert base64 to data URL for display
        const base64Image = data.data.base64Image;
        const outputFormat = data.data.outputFormat || 'jpg';
        const imageUrl = `data:image/${outputFormat};base64,${base64Image}`;
        
        return {
          imageUrl: imageUrl,
          success: true,
          message: data.message || 'Image generated successfully'
        };
      } else {
        // Handle error responses - extract errorCode from response body
        return {
          imageUrl: '',
          success: false,
          message: data.message || `Generation failed with status: ${response.status}`,
          errorCode: data.errorCode
        };
      }
    } catch (error) {
      console.error('Error generating photo:', error);
      return {
        imageUrl: '',
        success: false,
        message: 'Failed to generate photo'
      };
    }
  }, [getAuthToken]);

  // 4. Upload zip file
  const uploadZipFile = useCallback(async (zipFile: File, modelId: string): Promise<UploadZipResponse> => {
    try {
      console.log('Uploading zip file:', zipFile.name, 'for model:', modelId);
      
      const token = await getAuthToken();
      const headers: HeadersInit = {};
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
      
      const formData = new FormData();
      formData.append('file', zipFile);
      
      const response = await fetch(`${API_ROOT}/files/upload`, {
        method: 'POST',
        headers: headers,
        body: formData
      });
      console.log(response);
      if (response.ok) {
        const data = await response.json();
        
        if (data.isSuccess) {
          return {
            url: data.data,
            success: true,
            message: data.message || 'Zip file uploaded successfully'
          };
        } else {
          return {
            url: '',
            success: false,
            message: data.message || 'Upload failed'
          };
        }
      } else {
        return {
          url: '',
          success: false,
          message: `Upload failed with status: ${response.status}`
        };
      }
    } catch (error) {
      console.error('Error uploading zip file:', error);
      return {
        url: '',
        success: false,
        message: 'Failed to upload zip file'
      };
    }
  }, [getAuthToken]);

  // 5. Train model
  const trainModel = useCallback(async (request: TrainModelRequest): Promise<TrainModelResponse> => {
    try {
      console.log('Starting model training with request:', request);
      
      const token = await getAuthToken();
      const headers: HeadersInit = {
        'Content-Type': 'application/json',
      };
      
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
      
      const response = await fetch(`${API_ROOT}/integration/train-model`, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(request)
      });
      
      // Always try to parse the response body, regardless of status
      const data = await response.json();
      console.log('Train model API response data:', data);
      
      if (response.ok && data.isSuccess) {
        return {
          success: true,
          message: data.message || 'Model training started successfully',
          modelId: data.data
        };
      } else {
        // Handle error responses - extract errorCode from response body
        return {
          success: false,
          message: data.message || `Training failed with status: ${response.status}`,
          errorCode: data.errorCode
        };
      }
    } catch (error) {
      console.error('Error training model:', error);
      return {
        success: false,
        message: 'Failed to train model'
      };
    }
  }, [getAuthToken]);

  return {
    checkUserModelAvailable,
    generatePhoto,
    uploadZipFile,
    trainModel
  };
};
