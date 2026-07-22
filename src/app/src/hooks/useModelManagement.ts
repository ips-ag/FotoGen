
import { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { useApi } from '@/hooks/useApi';
import { useToast } from '@/hooks/use-toast';
import { User } from '@/hooks/useUserData';

interface ModelInfo {
  id: string;
  ownerName: string;
  isOwnedByUser: boolean;
}

export const useModelManagement = (user: User | null, isAuthenticated: boolean) => {
  const navigate = useNavigate();
  const { username, modelName } = useParams();
  const location = useLocation();
  const { toast } = useToast();
  const { checkUserModelAvailable } = useApi();
  
  const [userHasModel, setUserHasModel] = useState<boolean>(false);
  const [modelInfo, setModelInfo] = useState<ModelInfo | null>(null);
  const [isLoadingModel, setIsLoadingModel] = useState(true);

  console.log('useModelManagement hook - params:', { username, modelName });
  console.log('useModelManagement hook - location.pathname:', location.pathname);
  console.log('useModelManagement hook - isAuthenticated:', isAuthenticated);
  console.log('useModelManagement hook - user?.id:', user?.id);

  useEffect(() => {
    console.log('useModelManagement useEffect triggered');
    console.log('useModelManagement effect - username:', username, 'modelName:', modelName, 'pathname:', location.pathname);
    console.log('useModelManagement effect - isAuthenticated:', isAuthenticated, 'user?.id:', user?.id);
    
    // Wait for authentication to be available
    if (isAuthenticated && user?.id) {
      console.log('useModelManagement - All conditions met, proceeding with model check');
      // Check if we're coming from a shared model URL or if there's stored model info
      const storedModelInfo = sessionStorage.getItem('sharedModelInfo');
      console.log('useModelManagement - storedModelInfo:', storedModelInfo);
      
      if (username && modelName) {
        // When accessing another user's model via URL, use the modelName from params
        console.log('useModelManagement - Detected shared model URL - calling checkOtherUserModel with:', modelName, username);
        checkOtherUserModel(modelName, username);
      } else if (storedModelInfo && location.pathname === '/home') {
        // When on /home but we have stored shared model info, use it
        console.log('useModelManagement - Using stored shared model info on /home');
        const parsed = JSON.parse(storedModelInfo);
        setModelInfo(parsed);
        setUserHasModel(true);
        setIsLoadingModel(false);
      } else {
        // When accessing user's own model, let backend find it and clear any stored shared model
        console.log('useModelManagement - Accessing user own model - clearing stored shared model');
        sessionStorage.removeItem('sharedModelInfo');
        checkUserModel();
      }
    } else if (!isAuthenticated) {
      console.log('useModelManagement - User not authenticated, setting loading to false');
      setIsLoadingModel(false);
    }
  }, [isAuthenticated, user?.id, username, modelName, location.pathname, checkUserModelAvailable]);

  const checkOtherUserModel = async (modelId: string, ownerName: string) => {
    console.log('checkOtherUserModel called with:', { modelId, ownerName, userId: user?.id });
    
    if (!user?.id) {
      console.log('checkOtherUserModel - No user ID, returning');
      return;
    }
    
    setIsLoadingModel(true);
    try {
      // Pass the specific model ID from URL to the API
      console.log('checkOtherUserModel - Checking other user model with modelId:', modelId);
      const response = await checkUserModelAvailable(user.id, modelId);
      console.log('checkOtherUserModel - API response:', response);
      
      if (response.success && response.hasModel) {
        const sharedModelInfo = {
          id: modelId, // Use the model ID from URL params
          ownerName: ownerName,
          isOwnedByUser: false
        };
        
        console.log('checkOtherUserModel - Setting shared model info:', sharedModelInfo);
        setUserHasModel(true);
        setModelInfo(sharedModelInfo);
        
        // Store the shared model info for use when navigating to /home
        sessionStorage.setItem('sharedModelInfo', JSON.stringify(sharedModelInfo));
      } else {
        console.log('checkOtherUserModel - Model not found or not accessible');
        setUserHasModel(false);
        toast({
          title: 'Model Not Found',
          description: 'The requested model is not available or accessible.',
          variant: 'destructive',
        });
        navigate('/home');
      }
    } catch (error) {
      console.error('checkOtherUserModel - Error checking other user model:', error);
      toast({
        title: 'Error',
        description: 'Failed to check model availability',
        variant: 'destructive',
      });
      navigate('/home');
    } finally {
      setIsLoadingModel(false);
    }
  };

  const checkUserModel = async () => {
    console.log('checkUserModel called with userId:', user?.id);
    
    if (!user?.id) {
      console.log('checkUserModel - No user ID, returning');
      return;
    }
    
    setIsLoadingModel(true);
    try {
      // For user's own model, pass null to let backend find it
      console.log('checkUserModel - Checking user own model, passing null to API');
      const response = await checkUserModelAvailable(user.id, null);
      console.log('checkUserModel - API response:', response);
      
      if (response.success) {
        setUserHasModel(response.hasModel);
        if (response.hasModel && response.modelName) {
          setModelInfo({
            id: response.modelName,
            ownerName: user.displayName || user.name || 'IPS User',
            isOwnedByUser: true
          });
        }
      } else {
        console.error('checkUserModel - Failed to check user model:', response.message);
      }
    } catch (error) {
      console.error('checkUserModel - Error checking user model:', error);
    } finally {
      setIsLoadingModel(false);
    }
  };

  return {
    userHasModel,
    modelInfo,
    isLoadingModel
  };
};
