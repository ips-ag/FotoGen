
import React from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Upload } from 'lucide-react';

interface NoModelCardProps {
  onTrainModel: () => void;
}

export const NoModelCard = ({ onTrainModel }: NoModelCardProps) => {
  return (
    <div className="max-w-3xl mx-auto text-center space-y-8">
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-2">
          Train Your AI Model
        </h2>
        <p className="text-gray-600 text-lg">
          Before generating images, you need to train your personal AI model with your data
        </p>
      </div>

      <Card className="shadow-xl border-0 bg-white/80 backdrop-blur-sm">
        <CardContent className="p-12">
          <div className="space-y-6">
            <div className="p-8 rounded-full w-32 h-32 mx-auto flex items-center justify-center" style={{ background: `linear-gradient(to right, rgba(23, 66, 140, 0.1), rgba(18, 85, 151, 0.1))` }}>
              <Upload className="h-12 w-12" style={{ color: '#17428c' }} />
            </div>
            
            <div>
              <h3 className="text-xl font-semibold text-gray-900 mb-2">
                No Model Found
              </h3>
              <p className="text-gray-600 mb-6">
                You haven't trained your personal AI model yet. Upload your training data to get started.
              </p>
            </div>

            <Button 
              onClick={onTrainModel}
              size="lg"
              className="text-white font-semibold px-8 py-4 text-lg"
              style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
            >
              <Upload className="h-5 w-5 mr-2" />
              Start Training Your Model
            </Button>

            <div className="text-sm text-gray-500 space-y-1">
              <p>• Upload 10-20 high-quality images</p>
              <p>• Training typically takes 15-30 minutes</p>
              <p>• You'll receive an email when complete</p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};
