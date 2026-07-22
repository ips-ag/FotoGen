
import { useState } from 'react';
import { useApi } from '@/hooks/useApi';
import { useToast } from '@/hooks/use-toast';

interface ModelInfo {
  id: string;
  ownerName: string;
  isOwnedByUser: boolean;
}

export const useImageGeneration = (modelInfo: ModelInfo | null) => {
  const [prompt, setPrompt] = useState('Dressed in dark blue suit and white shirt. Torso, arms, and complete head are visible. Arms crossed. Portrait slightly from the side. Standing in front of a clean marble color wall.');
  const [isGenerating, setIsGenerating] = useState(false);
  const [generatedImage, setGeneratedImage] = useState<string | null>(null);
  const [showLimitationDialog, setShowLimitationDialog] = useState(false);
  const [limitationCount, setLimitationCount] = useState<string | number | null>(null);
  const { toast } = useToast();
  const { generatePhoto } = useApi();

  const handleGenerate = async () => {
    if (!prompt.trim() || !modelInfo) return;
    
    setIsGenerating(true);
    try {
      const response = await generatePhoto({
        ModelName: modelInfo.isOwnedByUser ? null : modelInfo.id,
        Prompt: prompt
      });
      
      console.log('Generate photo response:', response);
      
      if (response.success && response.imageUrl) {
        setGeneratedImage(response.imageUrl);
        toast({
          title: 'Success',
          description: 'Image generated successfully!',
        });
      } else {
        // Check if the error is about reaching generation limitations
        console.log('Checking errorCode:', response.errorCode);
        if (response.errorCode === 'ReachPhotoGenerationLimitation') {
          console.log('Setting showLimitationDialog to true');
          console.log('Limitation count from message:', response.message);
          setLimitationCount(response.message);
          setShowLimitationDialog(true);
        } else {
          throw new Error(response.message || 'Failed to generate image');
        }
      }
    } catch (error) {
      console.error('Error generating image:', error);
      toast({
        title: 'Generation Failed',
        description: 'Failed to generate image. Please try again.',
        variant: 'destructive',
      });
    } finally {
      setIsGenerating(false);
    }
  };

  return {
    prompt,
    setPrompt,
    isGenerating,
    generatedImage,
    handleGenerate,
    showLimitationDialog,
    setShowLimitationDialog,
    limitationCount
  };
};
