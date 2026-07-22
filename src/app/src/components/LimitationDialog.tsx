
import React from 'react';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { AlertTriangle } from 'lucide-react';

interface LimitationDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  limitationCount?: string | number;
  type: 'training' | 'generation';
}

export const LimitationDialog = ({ open, onOpenChange, limitationCount, type }: LimitationDialogProps) => {
  const displayCount = limitationCount || 2;
  
  const getTitle = () => {
    return type === 'training' ? 'Training Limit Reached' : 'Generation Limit Reached';
  };
  
  const getMessage = () => {
    if (type === 'training') {
      return `You've reached your training model limit of ${displayCount} images. Please try again later.`;
    } else {
      return `You've reached your photo generation limit of ${displayCount} images. Please try again later.`;
    }
  };
  
  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent className="sm:max-w-md">
        <AlertDialogHeader>
          <div className="flex items-center gap-2">
            <AlertTriangle className="h-5 w-5 text-orange-500" />
            <AlertDialogTitle>{getTitle()}</AlertDialogTitle>
          </div>
          <AlertDialogDescription className="text-left">
            {getMessage()}
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogAction 
            onClick={() => onOpenChange(false)}
            className="text-white font-semibold"
            style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
          >
            Understood
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
};
