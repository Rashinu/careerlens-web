interface ScoreGaugeProps {
  score: number;
  size?: 'sm' | 'lg';
}

function getColor(score: number): string {
  if (score <= 40) return '#ef4444';
  if (score <= 70) return '#f59e0b';
  return '#22c55e';
}

export function ScoreGauge({ score, size = 'sm' }: ScoreGaugeProps) {
  const dimension = size === 'lg' ? 160 : 100;
  const cx = dimension / 2;
  const cy = dimension / 2;
  const r = size === 'lg' ? 60 : 38;
  const strokeWidth = size === 'lg' ? 12 : 8;

  // Semicircle: starts at 180deg (left), ends at 0deg (right), top arc
  const circumference = Math.PI * r; // half circle arc length
  const progress = Math.max(0, Math.min(100, score)) / 100;
  const dashOffset = circumference * (1 - progress);
  const color = getColor(score);

  // Path for top semicircle (left to right)
  const startX = cx - r;
  const endX = cx + r;
  const pathD = `M ${startX} ${cy} A ${r} ${r} 0 0 1 ${endX} ${cy}`;

  return (
    <div className="flex flex-col items-center gap-1">
      <svg
        width={dimension}
        height={dimension / 2 + strokeWidth}
        aria-label={`Skor: ${score} üzerinden 100`}
        role="img"
      >
        {/* Background track */}
        <path
          d={pathD}
          fill="none"
          stroke="currentColor"
          strokeOpacity="0.15"
          strokeWidth={strokeWidth}
          strokeLinecap="round"
        />
        {/* Progress arc */}
        <path
          d={pathD}
          fill="none"
          stroke={color}
          strokeWidth={strokeWidth}
          strokeLinecap="round"
          strokeDasharray={circumference}
          strokeDashoffset={dashOffset}
          style={{ transition: 'stroke-dashoffset 0.6s ease' }}
        />
        {/* Score text */}
        <text
          x={cx}
          y={cy}
          textAnchor="middle"
          dominantBaseline="auto"
          fontSize={size === 'lg' ? 28 : 18}
          fontWeight="700"
          fill={color}
        >
          {score}
        </text>
      </svg>
      <span className="text-xs text-[var(--muted-foreground)]">/ 100</span>
    </div>
  );
}
