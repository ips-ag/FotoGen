import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Sparkles, Clock, Mail, Image, Upload, Loader2 } from 'lucide-react';
import { useApi } from '@/hooks/useApi';
import { useToast } from '@/hooks/use-toast';
import { useIsAuthenticated } from '@azure/msal-react';
import { AuthGuard } from '@/auth/AuthGuard';
import { LimitationDialog } from '@/components/LimitationDialog';
import { SizeErrorDialog } from '@/components/SizeErrorDialog';
import JSZip from 'jszip';

const TrainingPage = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const isAuthenticated = useIsAuthenticated();
  const { uploadZipFile, trainModel } = useApi();
  
  const [isTraining, setIsTraining] = useState(false);
  const [isUploading, setIsUploading] = useState(false);
  const [uploadedFiles, setUploadedFiles] = useState<File[]>([]);
  const [previewUrls, setPreviewUrls] = useState<string[]>([]);
  const [currentStep, setCurrentStep] = useState<'upload' | 'training' | 'complete'>('upload');
  const [showLimitationDialog, setShowLimitationDialog] = useState(false);
  const [limitationCount, setLimitationCount] = useState<string | number | null>(null);
  const [showSizeErrorDialog, setShowSizeErrorDialog] = useState(false);
  const [totalImageSize, setTotalImageSize] = useState(0);

  // Redirect to login if not authenticated
  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
  }, [isAuthenticated, navigate]);

  const generateModelId = () => {
    return 'model-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
  };

  const calculateTotalSize = (files: File[]): number => {
    return files.reduce((total, file) => total + file.size, 0);
  };

  const createZipFile = async (files: File[], modelId: string): Promise<File> => {
    const zip = new JSZip();
    
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      const fileExtension = file.name.split('.').pop();
      const fileName = `image_${i + 1}.${fileExtension}`;
      zip.file(fileName, file);
    }

    const zipBlob = await zip.generateAsync({ type: 'blob' });
    return new File([zipBlob], `${modelId}.zip`, { type: 'application/zip' });
  };

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (!files || files.length === 0) return;

    const fileArray = Array.from(files);
    const totalSize = calculateTotalSize(fileArray);
    const maxSize = 200 * 1024 * 1024; // 200MB in bytes

    // Check if total size exceeds 200MB
    if (totalSize > maxSize) {
      setTotalImageSize(totalSize);
      setShowSizeErrorDialog(true);
      // Clear the input
      event.target.value = '';
      return;
    }

    setUploadedFiles(fileArray);
    
    const urls = fileArray.map(file => URL.createObjectURL(file));
    setPreviewUrls(urls);
    
    toast({
      title: 'Images Selected',
      description: `${fileArray.length} images selected and ready for training.`,
    });
  };

  const handleSizeErrorDialogClose = () => {
    setShowSizeErrorDialog(false);
    // Clear any previously uploaded files and preview URLs
    setUploadedFiles([]);
    setPreviewUrls([]);
  };

  const handleStartTraining = async () => {
    if (!uploadedFiles || uploadedFiles.length === 0) {
      toast({
        title: 'No Images Selected',
        description: 'Please select images before starting training.',
        variant: 'destructive',
      });
      return;
    }

    setIsTraining(true);

    try {
      // Step 1: Upload files
      setCurrentStep('upload');
      const modelId = generateModelId();
      console.log('Generated model ID:', modelId);

      const zipFile = await createZipFile(uploadedFiles, modelId);
      console.log('Created zip file:', zipFile);

      const uploadResponse = await uploadZipFile(zipFile, modelId);
      
      if (!uploadResponse.success) {
        throw new Error(uploadResponse.message || 'Upload failed');
      }

      toast({
        title: 'Upload Successful',
        description: `${uploadedFiles.length} images uploaded successfully.`,
      });

      // Step 2: Start training
      setCurrentStep('training');
      const trainResponse = await trainModel({ imageUrl: uploadResponse.url });
      
      if (trainResponse.success) {
        setCurrentStep('complete');
        toast({
          title: 'Training Started',
          description: 'Your model training has begun successfully.',
        });
        
        // Small delay to show success message before transitioning
        setTimeout(() => {
          // Don't reset isTraining here, let the training state persist
        }, 1000);
      } else {
        // Check if the error is about reaching training limitations
        console.log('Checking training errorCode:', trainResponse.errorCode);
        if (trainResponse.errorCode === 'ReachTrainingLimitation') {
          console.log('Setting showLimitationDialog to true for training');
          console.log('Training limitation count from message:', trainResponse.message);
          setLimitationCount(trainResponse.message);
          setShowLimitationDialog(true);
          setIsTraining(false);
          setCurrentStep('upload');
        } else {
          throw new Error(trainResponse.message || 'Training failed to start');
        }
      }
    } catch (error) {
      console.error('Training error:', error);
      toast({
        title: 'Training Failed',
        description: 'Failed to start model training. Please try again.',
        variant: 'destructive',
      });
      setIsTraining(false);
      setCurrentStep('upload');
    }
  };

  if (!isAuthenticated) {
    return null; // Will redirect to login
  }

  return (
    <AuthGuard>
      <div className="min-h-screen bg-gradient-to-br from-slate-50 to-blue-50">
        {/* Header */}
        <header className="bg-white border-b border-gray-200">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="flex justify-between items-center h-16">
              <div className="flex items-center gap-3">
                <img 
                  src="https://www.ips-ag.com/wp-content/themes/ips-group-v1/images/ips-logo-no-claim.svg" 
                  alt="IPS Logo" 
                  className="h-8 cursor-pointer"
                  onClick={() => navigate('/home')}
                />
                <h1 
                  className="text-2xl font-bold cursor-pointer"
                  style={{ color: '#17428c' }}
                  onClick={() => navigate('/home')}
                >
                  FotoGen
                </h1>
              </div>
            </div>
          </div>
        </header>

        <main className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="text-center mb-8">
            <h2 className="text-3xl font-bold text-gray-900 mb-2">
              Train Your AI Model
            </h2>
            <p className="text-gray-600">
              Upload your training data to create a personalized AI model
            </p>
          </div>

          <Card className="shadow-xl border-0 bg-white/80 backdrop-blur-sm">
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Image className="h-5 w-5" style={{ color: '#17428c' }} />
                Training Images
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-6">
              {!isTraining ? (
                <>
                  {/* Upload Button */}
                  <div className="flex justify-center mb-6">
                    <div className="relative">
                      <Input
                        type="file"
                        multiple
                        accept="image/*"
                        onChange={handleFileUpload}
                        disabled={isUploading}
                        className="absolute inset-0 w-full h-full opacity-0 cursor-pointer"
                      />
                      <Button
                        disabled={isUploading}
                        className="text-white font-semibold px-6 py-3"
                        style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
                      >
                        <Upload className="h-4 w-4 mr-2" />
                        {isUploading ? 'Uploading...' : 'Upload Images'}
                      </Button>
                    </div>
                  </div>

                  {/* Uploaded Images Preview */}
                  {previewUrls.length > 0 && (
                    <div className="mb-6">
                      <h4 className="font-medium mb-3 text-gray-700">
                        Selected Images ({previewUrls.length} images)
                      </h4>
                      <div className="grid grid-cols-5 gap-4">
                        {previewUrls.map((url, index) => (
                          <div key={index} className="aspect-square">
                            <img
                              src={url}
                              alt={`Selected image ${index + 1}`}
                              className="w-full h-full object-cover rounded-lg border-2 border-green-300"
                            />
                          </div>
                        ))}
                      </div>
                    </div>
                  )}

                  {/* Upload Status */}
                  {uploadedFiles.length > 0 && (
                    <div className="p-4 rounded-lg border border-green-200" style={{ backgroundColor: 'rgba(34, 197, 94, 0.05)' }}>
                      <p className="text-sm text-green-700">
                        ✓ {uploadedFiles.length} images selected and ready for training
                      </p>
                    </div>
                  )}

                  <div className="p-4 rounded-lg border border-blue-200" style={{ backgroundColor: 'rgba(23, 66, 140, 0.05)' }}>
                    <h4 className="font-medium mb-2" style={{ color: '#17428c' }}>Training Requirements:</h4>
                    <ul className="text-sm space-y-1" style={{ color: '#125597' }}>
                      <li>• Upload 10-20 high-quality images</li>
                      <li>• Supported formats: JPG, PNG</li>
                      <li>• Maximum total size: 200MB</li>
                      <li>• Training takes approximately 20 minutes</li>
                    </ul>
                  </div>

                  <Button
                    onClick={handleStartTraining}
                    disabled={uploadedFiles.length === 0 || isTraining}
                    className="w-full text-white font-semibold py-3"
                    style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
                  >
                    <Sparkles className="h-4 w-4 mr-2" />
                    Start Training Model
                  </Button>
                </>
              ) : (
                <div className="text-center space-y-6">
                  <div className="p-8 rounded-full w-32 h-32 mx-auto flex items-center justify-center" style={{ background: `linear-gradient(to right, rgba(23, 66, 140, 0.1), rgba(18, 85, 151, 0.1))` }}>
                    {currentStep === 'upload' && (
                      <div className="flex flex-col items-center">
                        <Loader2 className="h-12 w-12 animate-spin" style={{ color: '#17428c' }} />
                        <span className="text-xs mt-2" style={{ color: '#17428c' }}>Uploading...</span>
                      </div>
                    )}
                    {currentStep === 'training' && (
                      <div className="flex flex-col items-center">
                        <Loader2 className="h-12 w-12 animate-spin" style={{ color: '#17428c' }} />
                        <span className="text-xs mt-2" style={{ color: '#17428c' }}>Training...</span>
                      </div>
                    )}
                    {currentStep === 'complete' && (
                      <Clock className="h-12 w-12 animate-pulse" style={{ color: '#17428c' }} />
                    )}
                  </div>
                  
                  <div>
                    <h3 className="text-xl font-semibold text-gray-900 mb-2">
                      {currentStep === 'upload' && 'Uploading Your Images'}
                      {currentStep === 'training' && 'Starting Model Training'}
                      {currentStep === 'complete' && 'Your Model is Training'}
                    </h3>
                    <p className="text-gray-600 mb-4">
                      {currentStep === 'upload' && 'Please wait while we upload your training images...'}
                      {currentStep === 'training' && 'Initializing the training process...'}
                      {currentStep === 'complete' && 'Your AI model training has started. This process takes approximately 20 minutes to complete.'}
                    </p>
                  </div>

                  {currentStep === 'complete' && (
                    <div className="p-4 rounded-lg border border-blue-200" style={{ backgroundColor: 'rgba(23, 66, 140, 0.05)' }}>
                      <div className="flex items-center gap-2 justify-center" style={{ color: '#125597' }}>
                        <Mail className="h-4 w-4" />
                        <span className="text-sm">You will receive an email notification when training is complete</span>
                      </div>
                    </div>
                  )}
                </div>
              )}
            </CardContent>
          </Card>
        </main>

        <LimitationDialog 
          open={showLimitationDialog}
          onOpenChange={setShowLimitationDialog}
          limitationCount={limitationCount}
          type="training"
        />

        <SizeErrorDialog
          open={showSizeErrorDialog}
          onOpenChange={handleSizeErrorDialogClose}
          totalSize={totalImageSize}
        />
      </div>
    </AuthGuard>
  );
};

export default TrainingPage;
