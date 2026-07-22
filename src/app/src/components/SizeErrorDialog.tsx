
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

interface SizeErrorDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  totalSize: number;
}

export const SizeErrorDialog = ({ open, onOpenChange, totalSize }: SizeErrorDialogProps) => {
  const formatFileSize = (bytes: number) => {
    return (bytes / (1024 * 1024)).toFixed(2);
  };
  
  return (
    <AlertDialog open={open} onOpenChange={onOpenChange}>
      <AlertDialogContent className="sm:max-w-md">
        <AlertDialogHeader>
          <div className="flex items-center gap-2">
            <AlertTriangle className="h-5 w-5 text-orange-500" />
            <AlertDialogTitle>Images Too Large</AlertDialogTitle>
          </div>
          <AlertDialogDescription className="text-left">
            The total size of selected images is {formatFileSize(totalSize)}MB, which exceeds the 200MB limit. Please select smaller images or fewer images.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogAction 
            onClick={() => onOpenChange(false)}
            className="text-white font-semibold"
            style={{ background: `linear-gradient(to right, #17428c, #125597)` }}
          >
            OK
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
};
