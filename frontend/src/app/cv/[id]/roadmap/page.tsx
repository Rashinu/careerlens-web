'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { Loader2 } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, CvAnalysisDto, RoadmapDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Skeleton } from '@/components/ui/skeleton';
import { ScoreGauge } from '@/components/shared/score-gauge';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';
import { SalaryShareForm, type SalaryShareFormInitialValues } from '@/components/shared/salary-share-form';
import { formatCurrency } from '@/lib/utils';

interface RoadmapFormData {
  targetPosition: string;
}

interface ParsedCvForPrefill {
  techStack?: string[];
  skills?: string[];
  yearsOfExperience?: number;
}

interface RecommendationItem {
  title?: string;
  description?: string;
  estimatedMonthlyImpact?: number | null;
}

interface CourseItem {
  name?: string;
  platform?: string;
  priority?: string;
}

interface GapAnalysisData {
  strengths?: string[];
  gaps?: string[];
  estimatedSalaryRange?: { min?: number | null; max?: number | null };
}

interface RecommendationsData {
  immediate?: RecommendationItem[];
  shortTerm?: RecommendationItem[];
  longTerm?: RecommendationItem[];
  courses?: CourseItem[];
}

function parseJsonSafe<T>(raw: string, fallback: T): T {
  try {
    return JSON.parse(raw) as T;
  } catch {
    return fallback;
  }
}

function GapAnalysisSection({ raw }: { raw: string }) {
  const data = parseJsonSafe<GapAnalysisData>(raw, {});
  const hasContent =
    (data.strengths?.length ?? 0) > 0 ||
    (data.gaps?.length ?? 0) > 0 ||
    data.estimatedSalaryRange?.min != null;

  if (!hasContent) {
    return <p className="text-sm text-[var(--muted-foreground)]">Veri bulunamadı.</p>;
  }

  return (
    <div className="flex flex-col gap-4">
      {data.estimatedSalaryRange?.min != null && data.estimatedSalaryRange?.max != null && (
        <div className="rounded-lg border border-blue-200 bg-blue-50 dark:bg-blue-950 dark:border-blue-800 p-3">
          <p className="text-xs text-[var(--muted-foreground)]">Hedef pozisyon için tahmini maaş aralığı</p>
          <p className="text-base font-semibold text-[#2563EB]">
            {formatCurrency(data.estimatedSalaryRange.min)} — {formatCurrency(data.estimatedSalaryRange.max)}
          </p>
        </div>
      )}
      {data.strengths && data.strengths.length > 0 && (
        <div>
          <p className="text-xs font-medium text-[var(--muted-foreground)] mb-1.5">Güçlü Yönler</p>
          <div className="flex flex-wrap gap-2">
            {data.strengths.map((s, i) => (
              <span key={i} className="text-xs bg-green-50 dark:bg-green-950 text-green-700 dark:text-green-400 px-2 py-1 rounded-full">
                {s}
              </span>
            ))}
          </div>
        </div>
      )}
      {data.gaps && data.gaps.length > 0 && (
        <div>
          <p className="text-xs font-medium text-[var(--muted-foreground)] mb-1.5">Geliştirilmesi Gerekenler</p>
          <div className="flex flex-wrap gap-2">
            {data.gaps.map((g, i) => (
              <span key={i} className="text-xs bg-orange-50 dark:bg-orange-950 text-orange-700 dark:text-orange-400 px-2 py-1 rounded-full">
                {g}
              </span>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

function RecommendationGroup({ title, items }: { title: string; items?: RecommendationItem[] }) {
  if (!items || items.length === 0) return null;
  return (
    <div className="flex flex-col gap-2">
      <p className="text-xs font-medium text-[var(--muted-foreground)]">{title}</p>
      {items.map((item, i) => (
        <div key={i} className="flex items-start justify-between gap-3 rounded-lg border border-[var(--border)] p-3">
          <div>
            <p className="text-sm font-medium">{item.title ?? `Adım ${i + 1}`}</p>
            {item.description && (
              <p className="text-xs text-[var(--muted-foreground)] mt-0.5">{item.description}</p>
            )}
          </div>
          {item.estimatedMonthlyImpact != null && (
            <span className="shrink-0 text-xs font-semibold text-green-700 dark:text-green-400 bg-green-50 dark:bg-green-950 px-2 py-1 rounded-full">
              +{formatCurrency(item.estimatedMonthlyImpact)}/ay
            </span>
          )}
        </div>
      ))}
    </div>
  );
}

function RecommendationsTimeline({ raw }: { raw: string }) {
  const data = parseJsonSafe<RecommendationsData>(raw, {});
  const hasContent =
    (data.immediate?.length ?? 0) > 0 ||
    (data.shortTerm?.length ?? 0) > 0 ||
    (data.longTerm?.length ?? 0) > 0 ||
    (data.courses?.length ?? 0) > 0;

  if (!hasContent) {
    return <p className="text-sm text-[var(--muted-foreground)]">Veri bulunamadı.</p>;
  }

  return (
    <div className="flex flex-col gap-5">
      <RecommendationGroup title="Hemen Yapılacaklar" items={data.immediate} />
      <RecommendationGroup title="3-6 Ay İçinde" items={data.shortTerm} />
      <RecommendationGroup title="6-12 Ay İçinde" items={data.longTerm} />
      {data.courses && data.courses.length > 0 && (
        <div className="flex flex-col gap-2">
          <p className="text-xs font-medium text-[var(--muted-foreground)]">Önerilen Kurslar</p>
          <div className="flex flex-wrap gap-2">
            {data.courses.map((c, i) => (
              <span key={i} className="text-xs bg-[var(--muted)] px-2 py-1 rounded-full">
                {c.name} {c.platform ? `— ${c.platform}` : ''}
              </span>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default function RoadmapPage() {
  const params = useParams<{ id: string }>();
  const [roadmap, setRoadmap] = useState<RoadmapDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [salaryPrefill, setSalaryPrefill] = useState<SalaryShareFormInitialValues>({});
  const [salaryCardDismissed, setSalaryCardDismissed] = useState(false);

  const { register, handleSubmit, formState: { errors } } = useForm<RoadmapFormData>();

  async function fetchCvForPrefill() {
    try {
      const res = await apiClient.get<ApiResponse<CvAnalysisDto>>(`/api/cv/${params.id}/analysis`);
      if (!res.data.success || !res.data.data.parsedData) return;
      const parsed = JSON.parse(res.data.data.parsedData) as ParsedCvForPrefill;
      setSalaryPrefill({
        techStack: parsed.techStack ?? parsed.skills,
        yearsOfExperience: parsed.yearsOfExperience,
      });
    } catch {
      // ön-doldurma opsiyonel; başarısız olursa boş formla devam edilir
    }
  }

  async function fetchRoadmap() {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<RoadmapDto>>(
        `/api/cv/${params.id}/roadmap`
      );
      if (res.data.success) {
        setRoadmap(res.data.data);
        fetchCvForPrefill();
      }
    } catch (err: unknown) {
      const status = (err as { response?: { status?: number } })?.response?.status;
      if (status !== 404) {
        setError('Yol haritası yüklenirken hata oluştu.');
      }
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchRoadmap();
  }, [params.id]);

  async function onSubmit(data: RoadmapFormData) {
    setGenerating(true);
    try {
      const res = await apiClient.post<ApiResponse<RoadmapDto>>(
        `/api/cv/${params.id}/roadmap`,
        { targetPosition: data.targetPosition }
      );
      if (res.data.success) {
        setRoadmap(res.data.data);
        fetchCvForPrefill();
        toast.success('Yol haritanız oluşturuldu!');
      } else {
        toast.error(res.data.error ?? 'Yol haritası oluşturulamadı.');
      }
    } catch {
      toast.error('Yol haritası oluşturulurken hata oluştu.');
    } finally {
      setGenerating(false);
    }
  }

  if (loading) {
    return (
      <div className="mx-auto max-w-3xl px-4 py-10 flex flex-col gap-4">
        <Skeleton className="h-8 w-56" />
        <Skeleton className="h-48 w-full" />
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-3xl px-4 py-10">
      <h1 className="text-2xl font-bold mb-8">Kariyer Yol Haritası</h1>

      {error && (
        <div className="mb-6">
          <ApiErrorAlert message={error} onRetry={fetchRoadmap} />
        </div>
      )}

      {!roadmap ? (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Hedef Pozisyon Belirle</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="targetPosition">Hedef Pozisyon</Label>
                <Input
                  id="targetPosition"
                  placeholder="Örn: Senior Backend Developer"
                  {...register('targetPosition', {
                    required: 'Hedef pozisyon zorunludur.',
                  })}
                />
                {errors.targetPosition && (
                  <p className="text-xs text-[var(--destructive)]">
                    {errors.targetPosition.message}
                  </p>
                )}
              </div>
              <Button type="submit" disabled={generating} className="w-full">
                {generating ? (
                  <>
                    <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                    Yol Haritası Oluşturuluyor...
                  </>
                ) : (
                  'Yol Haritası Oluştur'
                )}
              </Button>
            </form>
          </CardContent>
        </Card>
      ) : (
        <div className="flex flex-col gap-6">
          {/* Score */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Mevcut Skorunuz</CardTitle>
            </CardHeader>
            <CardContent className="flex items-center gap-8 flex-wrap">
              <ScoreGauge score={roadmap.currentScore} size="lg" />
              <div>
                <p className="text-sm text-[var(--muted-foreground)]">Hedef Pozisyon</p>
                <p className="text-lg font-semibold">{roadmap.targetPosition}</p>
              </div>
            </CardContent>
          </Card>

          {/* Gap Analysis */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Gelişim Alanları</CardTitle>
            </CardHeader>
            <CardContent>
              <GapAnalysisSection raw={roadmap.gapAnalysis} />
            </CardContent>
          </Card>

          {/* Recommendations */}
          <Card>
            <CardHeader>
              <CardTitle className="text-base">Önerilen Adımlar</CardTitle>
            </CardHeader>
            <CardContent>
              <RecommendationsTimeline raw={roadmap.recommendations} />
            </CardContent>
          </Card>

          {/* Maaş paylaşım daveti */}
          {!salaryCardDismissed && (
            <Card>
              <CardHeader className="flex items-start justify-between flex-row gap-2">
                <div>
                  <CardTitle className="text-base">Verini Paylaş, Topluluğu Güçlendir</CardTitle>
                  <p className="text-xs text-[var(--muted-foreground)] mt-1">
                    Yol haritanı aldın — maaşını anonim paylaşarak diğer kullanıcıların daha doğru
                    benchmark görmesine yardımcı olabilirsin.
                  </p>
                </div>
                <Button variant="ghost" size="sm" onClick={() => setSalaryCardDismissed(true)}>
                  Atla
                </Button>
              </CardHeader>
              <CardContent>
                <SalaryShareForm
                  initialValues={{ position: roadmap.targetPosition, ...salaryPrefill }}
                  title="Maaş Bilgilerini Paylaş"
                  description="Hedef pozisyonun ve tahmini deneyimin ön dolduruldu, gerekirse düzenleyebilirsin"
                  onSubmitted={() => setSalaryCardDismissed(true)}
                />
              </CardContent>
            </Card>
          )}
        </div>
      )}
    </div>
  );
}
