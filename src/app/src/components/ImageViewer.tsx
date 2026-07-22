
import React, { useState, useRef, useCallback } from 'react';
import { Button } from '@/components/ui/button';
import { ZoomIn, ZoomOut, Download } from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

interface ImageViewerProps {
  src: string;
  alt: string;
  className?: string;
}

export const ImageViewer = ({ src, alt, className = "" }: ImageViewerProps) => {
  const [zoom, setZoom] = useState(100);
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [isDragging, setIsDragging] = useState(false);
  const [dragStart, setDragStart] = useState({ x: 0, y: 0 });
  const { toast } = useToast();
  const imageRef = useRef<HTMLImageElement>(null);

  const handleZoomIn = () => {
    if (zoom < 200) {
      setZoom(prev => Math.min(prev + 25, 200));
    }
  };

  const handleZoomOut = () => {
    if (zoom > 50) {
      const newZoom = Math.max(zoom - 25, 50);
      setZoom(newZoom);
      // Reset position when zooming out to 100% or less
      if (newZoom <= 100) {
        setPosition({ x: 0, y: 0 });
      }
    }
  };

  const handleMouseDown = useCallback((e: React.MouseEvent) => {
    if (zoom > 100) {
      setIsDragging(true);
      setDragStart({
        x: e.clientX - position.x,
        y: e.clientY - position.y
      });
      e.preventDefault();
    }
  }, [zoom, position]);

  const handleMouseMove = useCallback((e: React.MouseEvent) => {
    if (isDragging && zoom > 100) {
      const newX = e.clientX - dragStart.x;
      const newY = e.clientY - dragStart.y;
      
      // Add constraints to prevent dragging too far
      const maxOffset = (zoom - 100) * 2;
      const constrainedX = Math.max(Math.min(newX, maxOffset), -maxOffset);
      const constrainedY = Math.max(Math.min(newY, maxOffset), -maxOffset);
      
      setPosition({ x: constrainedX, y: constrainedY });
    }
  }, [isDragging, dragStart, zoom]);

  const handleMouseUp = useCallback(() => {
    setIsDragging(false);
  }, []);

  const handleDownload = async () => {
    try {
      const response = await fetch(src);
      const blob = await response.blob();
      const url = URL.createObjectURL(blob);
      
      const link = document.createElement('a');
      link.href = url;
      link.download = `generated-image-${Date.now()}.png`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      
      URL.revokeObjectURL(url);
      
      toast({
        title: 'Success',
        description: 'Image downloaded successfully!',
      });
    } catch (error) {
      console.error('Error downloading image:', error);
      toast({
        title: 'Download Failed',
        description: 'Failed to download image. Please try again.',
        variant: 'destructive',
      });
    }
  };

  return (
    <div className={`relative ${className}`}>
      <div className="absolute top-2 right-2 z-10 flex gap-1">
        <Button
          size="sm"
          variant="secondary"
          onClick={handleZoomOut}
          disabled={zoom <= 50}
          className="h-8 w-8 p-0"
        >
          <ZoomOut className="h-4 w-4" />
        </Button>
        <Button
          size="sm"
          variant="secondary"
          onClick={handleZoomIn}
          disabled={zoom >= 200}
          className="h-8 w-8 p-0"
        >
          <ZoomIn className="h-4 w-4" />
        </Button>
        <Button
          size="sm"
          variant="secondary"
          onClick={handleDownload}
          className="h-8 w-8 p-0"
        >
          <Download className="h-4 w-4" />
        </Button>
      </div>
      
      <div 
        className="overflow-hidden max-h-full"
        onMouseMove={handleMouseMove}
        onMouseUp={handleMouseUp}
        onMouseLeave={handleMouseUp}
      >
        <img 
          ref={imageRef}
          src={src} 
          alt={alt}
          className={`w-full h-full object-cover rounded-lg transition-transform duration-200 ${
            zoom > 100 ? 'cursor-grab active:cursor-grabbing' : ''
          } ${isDragging ? 'cursor-grabbing' : ''}`}
          style={{ 
            transform: `scale(${zoom / 100}) translate(${position.x}px, ${position.y}px)`,
            transformOrigin: 'center'
          }}
          onMouseDown={handleMouseDown}
          draggable={false}
        />
      </div>
      
      <div className="absolute bottom-2 left-2 bg-black/60 text-white px-2 py-1 rounded text-xs">
        {zoom}%
      </div>
    </div>
  );
};
