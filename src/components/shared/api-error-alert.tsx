import { AlertCircle } from 'lucide-react';
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert';
import { Button } from '@/components/ui/button';

interface ApiErrorAlertProps {
  message: string;
  onRetry?: () => void;
}

export function ApiErrorAlert({ message, onRetry }: ApiErrorAlertProps) {
  return (
    <Alert variant="destructive">
      <AlertCircle className="h-4 w-4" />
      <AlertTitle>Hata</AlertTitle>
      <AlertDescription className="flex items-center justify-between gap-4">
        <span>{message}</span>
        {onRetry && (
          <Button variant="outline" size="sm" onClick={onRetry}>
            Tekrar Dene
          </Button>
        )}
      </AlertDescription>
    </Alert>
  );
}
