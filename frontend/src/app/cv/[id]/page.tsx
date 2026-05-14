'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import Link from 'next/link';
import { ArrowRight, CheckCircle } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, CvAnalysisDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';
import { Badge } from '@/components/ui/badge';
import { StatusBadge } from '@/components/shared/status-badge';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';
import { getStatusLabel } from '@/lib/utils';

interface ParsedCvData {
  name?: string;
  email?: string;
  phone?: string;
  summary?: string;
  skills?: string[];
  languages?: Array<{ name: string; level: string }>;
  education?: Array<{ institution: string; degree: string; year?: string }>;
  experience?: Array<{ company: string; role: string; duration?: string }>;
}

const PIPELINE_STEPS = ['Uploaded', 'TextExtracted', 'Analyzed'];

function PipelineStepper({ currentStatus }: { currentStatus: string }) {
  const currentIdx = PIPELINE_STEPS.indexOf(currentStatus);
  return (
    <div className="flex items-center gap-2 py-4">
      {PIPELINE_STEPS.map((step, idx) => {
        const isDone = currentIdx > idx;
        const isActive = currentStatus === step;
        return (
          <div key={step} className="flex items-center gap-2">
            <div
              className={`flex h-8 w-8 items-center justify-center rounded-full text-xs font-bold ${
                isDone
                  ? 'bg-green-500 text-white'
                  : isActive
                  ? 'bg-[#2563EB] text-white'
                  : 'bg-[var(--muted)] text-[var(--muted-foreground)]'
              }`}
            >
              {isDone ? <CheckCircle className="h-4 w-4" /> : idx + 1}
            </div>
            <span className="text-xs hidden sm:inline">{getStatusLabel(step)}</span>
            {idx < PIPELINE_STEPS.length - 1 && (
              <div className="h-px w-6 bg-[var(--border)]" />
            )}
          </div>
        );
      })}
    </div>
  );
}

function CvAnalysisView({ cv, parsed }: { cv: CvAnalysisDto; parsed: ParsedCvData }) {
  return (
    <div className="flex flex-col gap-6">
      {/* Header */}
      <div className="flex items-center justify-between flex-wrap gap-3">
        <div>
          <h1 className="text-xl font-bold">{parsed.name ?? cv.originalFileName}</h1>
          {parsed.email && (
            <p className="text-sm text-[var(--muted-foreground)]">{parsed.email}</p>
          )}
        </div>
        <div className="flex items-center gap-3">
          <StatusBadge status={cv.status} />
          <Button asChild>
            <Link href={`/cv/${cv.id}/roadmap`}>
              Yol Haritası Oluştur
              <ArrowRight className="h-4 w-4 ml-1" />
            </Link>
          </Button>
        </div>
      </div>

      {/* Summary */}
      {parsed.summary && (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Özet</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm leading-relaxed">{parsed.summary}</p>
          </CardContent>
        </Card>
      )}

      {/* Tech Stack */}
      {parsed.skills && parsed.skills.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Yetenekler & Teknolojiler</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex flex-wrap gap-2">
              {parsed.skills.map((skill) => (
                <Badge key={skill} variant="secondary">
                  {skill}
                </Badge>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Languages */}
      {parsed.languages && parsed.languages.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Dil Bilgisi</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex flex-wrap gap-2">
              {parsed.languages.map((lang) => (
                <Badge key={lang.name} variant="outline">
                  {lang.name} — {lang.level}
                </Badge>
              ))}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Education */}
      {parsed.education && parsed.education.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Eğitim</CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col gap-3">
            {parsed.education.map((edu, i) => (
              <div key={i} className="flex flex-col">
                <span className="text-sm font-medium">{edu.degree}</span>
                <span className="text-xs text-[var(--muted-foreground)]">
                  {edu.institution}{edu.year ? ` — ${edu.year}` : ''}
                </span>
              </div>
            ))}
          </CardContent>
        </Card>
      )}

      {/* Experience */}
      {parsed.experience && parsed.experience.length > 0 && (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Deneyim</CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col gap-3">
            {parsed.experience.map((exp, i) => (
              <div key={i} className="flex flex-col">
                <span className="text-sm font-medium">{exp.role}</span>
                <span className="text-xs text-[var(--muted-foreground)]">
                  {exp.company}{exp.duration ? ` — ${exp.duration}` : ''}
                </span>
              </div>
            ))}
          </CardContent>
        </Card>
      )}
    </div>
  );
}

export default function CvAnalysisPage() {
  const params = useParams<{ id: string }>();
  const [cv, setCv] = useState<CvAnalysisDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  async function fetchCv() {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<CvAnalysisDto>>(
        `/api/cv/${params.id}/analysis`
      );
      if (res.data.success) {
        setCv(res.data.data);
      } else {
        setError(res.data.error ?? 'Analiz verisi alınamadı.');
      }
    } catch {
      setError('Analiz yüklenirken hata oluştu.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchCv();
  }, [params.id]);

  if (loading) {
    return (
      <div className="mx-auto max-w-3xl px-4 py-10 flex flex-col gap-4">
        <Skeleton className="h-8 w-64" />
        <Skeleton className="h-32 w-full" />
        <Skeleton className="h-24 w-full" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="mx-auto max-w-3xl px-4 py-10">
        <ApiErrorAlert message={error} onRetry={fetchCv} />
      </div>
    );
  }

  if (!cv) return null;

  if (cv.status !== 'Analyzed') {
    return (
      <div className="mx-auto max-w-3xl px-4 py-10">
        <h1 className="text-xl font-bold mb-4">{cv.originalFileName}</h1>
        <Card>
          <CardHeader>
            <CardTitle className="text-base">İşlem Sürüyor</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-[var(--muted-foreground)] mb-4">
              CV analizi tamamlanmadı. Mevcut durum:
            </p>
            <PipelineStepper currentStatus={cv.status} />
          </CardContent>
        </Card>
      </div>
    );
  }

  let parsed: ParsedCvData = {};
  try {
    if (cv.parsedData) parsed = JSON.parse(cv.parsedData) as ParsedCvData;
  } catch {
    // parsedData geçerli JSON değilse boş nesne kullan
  }

  return (
    <div className="mx-auto max-w-3xl px-4 py-10">
      <CvAnalysisView cv={cv} parsed={parsed} />
    </div>
  );
}
