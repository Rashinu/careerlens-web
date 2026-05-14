import { Badge } from '@/components/ui/badge';
import { getStatusLabel } from '@/lib/utils';

type StatusVariant = 'gray' | 'amber' | 'green' | 'red' | 'default';

const statusVariantMap: Record<string, StatusVariant> = {
  Uploaded: 'gray',
  TextExtracted: 'amber',
  Analyzed: 'green',
  Failed: 'red',
};

interface StatusBadgeProps {
  status: string;
}

export function StatusBadge({ status }: StatusBadgeProps) {
  const variant = statusVariantMap[status] ?? 'default';
  return <Badge variant={variant}>{getStatusLabel(status)}</Badge>;
}
