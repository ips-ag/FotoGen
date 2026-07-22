
import React from 'react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { User, Users, RefreshCw, ArrowLeft } from 'lucide-react';

interface ModelInfo {
  id: string;
  ownerName: string;
  isOwnedByUser: boolean;
}

interface ModelInfoProps {
  modelInfo: ModelInfo;
  onTrainModel: () => void;
  onSwitchToUserModel: () => void;
}

export const ModelInfoSection = ({ modelInfo, onTrainModel, onSwitchToUserModel }: ModelInfoProps) => {
  return (
    <div className="flex justify-center items-center gap-4">
      <Badge variant="outline" className="flex items-center gap-2 px-3 py-1">
        {modelInfo.isOwnedByUser ? (
          <User className="h-4 w-4" style={{ color: '#125597' }} />
        ) : (
          <Users className="h-4 w-4" style={{ color: '#17428c' }} />
        )}
        <span className="text-sm">
          Model by: <strong>{modelInfo.ownerName}</strong>
        </span>
      </Badge>
      
      {modelInfo.isOwnedByUser ? (
        <Button 
          onClick={onTrainModel}
          variant="outline"
          size="sm"
          className="flex items-center gap-2 border-2"
          style={{ color: '#17428c', borderColor: '#17428c' }}
        >
          <RefreshCw className="h-4 w-4" />
          Retrain Model
        </Button>
      ) : (
        <Button 
          onClick={onSwitchToUserModel}
          variant="outline"
          size="sm"
          className="flex items-center gap-2 border-2"
          style={{ color: '#125597', borderColor: '#125597' }}
        >
          <ArrowLeft className="h-4 w-4" />
          Use My Model
        </Button>
      )}
    </div>
  );
};
