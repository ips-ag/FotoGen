
import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Zap, Sparkles, Loader2 } from 'lucide-react';

interface ImageGeneratorProps {
  prompt: string;
  onPromptChange: (value: string) => void;
  onGenerate: () => void;
  isGenerating: boolean;
}

export const ImageGenerator = ({ prompt, onPromptChange, onGenerate, isGenerating }: ImageGeneratorProps) => {
  return (
    <Card className="shadow-lg border-0 bg-white/80 backdrop-blur-sm">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Zap className="h-5 w-5" style={{ color: '#17428c' }} />
          Prompt Generator
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Describe your image
          </label>
          <Textarea
            placeholder="A beautiful sunset over mountains, photorealistic, high detail..."
            value={prompt}
            onChange={(e) => onPromptChange(e.target.value)}
            className="min-h-32 resize-none"
          />
        </div>
        
        <Button 
          onClick={onGenerate}
          disabled={!prompt.trim() || isGenerating}
          className="w-full text-white font-semibold py-3"
          style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
        >
          {isGenerating ? (
            <>
              <Loader2 className="h-4 w-4 mr-2 animate-spin" />
              Generating...
            </>
          ) : (
            <>
              <Sparkles className="h-4 w-4 mr-2" />
              Generate Image
            </>
          )}
        </Button>
      </CardContent>
    </Card>
  );
};
