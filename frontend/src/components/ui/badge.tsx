import * as React from 'react';
import { cva, type VariantProps } from 'class-variance-authority';
import { cn } from '@/lib/utils';

const badgeVariants = cva(
  'inline-flex items-center rounded-md border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-[var(--ring)] focus:ring-offset-2',
  {
    variants: {
      variant: {
        default:
          'border-transparent bg-[#2563EB] text-white shadow hover:bg-[#1d4ed8]',
        secondary:
          'border-transparent bg-[var(--muted)] text-[var(--foreground)] hover:opacity-80',
        destructive:
          'border-transparent bg-[var(--destructive)] text-[var(--destructive-foreground)] shadow hover:opacity-90',
        outline: 'text-[var(--foreground)]',
        amber:
          'border-transparent bg-amber-100 text-amber-800 dark:bg-amber-900 dark:text-amber-100',
        green:
          'border-transparent bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-100',
        gray:
          'border-transparent bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-300',
        red:
          'border-transparent bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-100',
      },
    },
    defaultVariants: {
      variant: 'default',
    },
  }
);

export interface BadgeProps
  extends React.HTMLAttributes<HTMLDivElement>,
    VariantProps<typeof badgeVariants> {}

function Badge({ className, variant, ...props }: BadgeProps) {
  return (
    <div className={cn(badgeVariants({ variant }), className)} {...props} />
  );
}

export { Badge, badgeVariants };
