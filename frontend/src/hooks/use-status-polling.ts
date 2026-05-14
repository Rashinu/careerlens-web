import { useEffect, useRef, useState } from 'react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, CvAnalysisDto } from '@/types/api';

interface PollingState {
  status: string | null;
  loading: boolean;
  error: string | null;
}

export function useStatusPolling(
  cvId: string | null,
  intervalMs: number
): PollingState {
  const [state, setState] = useState<PollingState>({
    status: null,
    loading: false,
    error: null,
  });
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

  useEffect(() => {
    if (!cvId) return;

    const TERMINAL = ['Analyzed', 'Failed'];

    async function poll() {
      try {
        setState((prev) => ({ ...prev, loading: true }));
        const res = await apiClient.get<ApiResponse<CvAnalysisDto>>(
          `/api/cv/${cvId}/analysis`
        );
        const status = res.data.data?.status ?? null;
        setState({ status, loading: false, error: null });
        if (status && TERMINAL.includes(status) && timerRef.current) {
          clearInterval(timerRef.current);
          timerRef.current = null;
        }
      } catch {
        setState((prev) => ({
          ...prev,
          loading: false,
          error: 'Durum alınamadı.',
        }));
      }
    }

    poll();
    timerRef.current = setInterval(poll, intervalMs);

    return () => {
      if (timerRef.current) {
        clearInterval(timerRef.current);
        timerRef.current = null;
      }
    };
  }, [cvId, intervalMs]);

  return state;
}
