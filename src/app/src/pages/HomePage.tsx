
import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Loader2 } from 'lucide-react';
import { useMsal, useIsAuthenticated } from '@azure/msal-react';
import { AuthGuard } from '@/auth/AuthGuard';
import { useUserData } from '@/hooks/useUserData';
import { useModelManagement } from '@/hooks/useModelManagement';
import { useImageGeneration } from '@/hooks/useImageGeneration';
import { Header } from '@/components/Header';
import { ModelInfoSection } from '@/components/ModelInfo';
import { ImageGenerator } from '@/components/ImageGenerator';
import { GeneratedImage } from '@/components/GeneratedImage';
import { NoModelCard } from '@/components/NoModelCard';
import { LimitationDialog } from '@/components/LimitationDialog';

const HomePage = () => {
  const navigate = useNavigate();
  const { instance, accounts } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const { user } = useUserData();
  
  const { userHasModel, modelInfo, isLoadingModel } = useModelManagement(user, isAuthenticated);
  const { 
    prompt, 
    setPrompt, 
    isGenerating, 
    generatedImage, 
    handleGenerate,
    showLimitationDialog,
    setShowLimitationDialog,
    limitationCount
  } = useImageGeneration(modelInfo);

  // Add debug logging
  console.log('HomePage - showLimitationDialog:', showLimitationDialog);

  // Redirect to login if not authenticated
  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
  }, [isAuthenticated, navigate]);

  const handleTrainModel = () => {
    navigate('/training');
  };

  const handleLogout = () => {
    if (accounts.length > 0) {
      instance.clearCache();
      window.location.reload();
    }
  };

  const handleSwitchToUserModel = () => {
    console.log('HomePage - Switch to user model clicked');
    // Clear any stored shared model info
    sessionStorage.removeItem('sharedModelInfo');
    console.log('HomePage - Cleared sharedModelInfo from sessionStorage');
    // Navigate to home and force a reload of user's own model
    navigate('/home', { replace: true });
    // Refresh the page to ensure clean state
    window.location.reload();
  };

  if (!isAuthenticated) {
    return null; // Will redirect to login
  }

  if (isLoadingModel) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50 flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin mx-auto mb-4" style={{ color: '#17428c' }} />
          <p className="text-gray-600">Checking your model...</p>
        </div>
      </div>
    );
  }

  return (
    <AuthGuard>
      <div className="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50">
        <Header user={user} onLogout={handleLogout} />

        <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 pt-24">
          {userHasModel && modelInfo ? (
            <div className="space-y-8">
              <div className="text-center">
                <h2 className="text-3xl font-bold text-gray-900 mb-2">
                  Generate Your Images
                </h2>
                <p className="text-gray-600">
                  Use {modelInfo.isOwnedByUser ? 'your trained model' : `${modelInfo.ownerName}'s model`} to create amazing images from text prompts
                </p>
              </div>

              <ModelInfoSection 
                modelInfo={modelInfo}
                onTrainModel={handleTrainModel}
                onSwitchToUserModel={handleSwitchToUserModel}
              />

              <div className="grid lg:grid-cols-2 gap-8">
                <ImageGenerator 
                  prompt={prompt}
                  onPromptChange={setPrompt}
                  onGenerate={handleGenerate}
                  isGenerating={isGenerating}
                />

                <GeneratedImage 
                  generatedImage={generatedImage}
                  isGenerating={isGenerating}
                />
              </div>
            </div>
          ) : (
            <NoModelCard onTrainModel={handleTrainModel} />
          )}
        </main>

        <LimitationDialog 
          open={showLimitationDialog}
          onOpenChange={setShowLimitationDialog}
          limitationCount={limitationCount}
          type="generation"
        />
      </div>
    </AuthGuard>
  );
};

export default HomePage;
