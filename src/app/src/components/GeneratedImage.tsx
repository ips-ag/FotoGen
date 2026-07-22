
import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Image as ImageIcon, Loader2 } from 'lucide-react';
import { ImageViewer } from './ImageViewer';

interface GeneratedImageProps {
  generatedImage: string | null;
  isGenerating: boolean;
}

export const GeneratedImage = ({ generatedImage, isGenerating }: GeneratedImageProps) => {
  return (
    <Card className="shadow-lg border-0 bg-white/80 backdrop-blur-sm">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <ImageIcon className="h-5 w-5" style={{ color: '#125597' }} />
          Generated Image
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="aspect-square bg-gray-100 rounded-lg flex items-center justify-center overflow-hidden">
          {isGenerating ? (
            <div className="text-center">
              <Loader2 className="h-8 w-8 animate-spin mx-auto mb-2" style={{ color: '#17428c' }} />
              <p className="text-sm text-gray-500">Creating your image...</p>
            </div>
          ) : generatedImage ? (
            <ImageViewer 
              src={generatedImage} 
              alt="Generated" 
              className="w-full h-full"
            />
          ) : (
            <div className="text-center text-gray-400">
              <ImageIcon className="h-12 w-12 mx-auto mb-2" />
              <p>Your generated image will appear here</p>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
};
