'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { Upload, FileText } from 'lucide-react';
import apiClient from '@/lib/api-client';
import type { ApiResponse, CvAnalysisListItemDto } from '@/types/api';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';
import { StatusBadge } from '@/components/shared/status-badge';
import { ApiErrorAlert } from '@/components/shared/api-error-alert';
import { formatDate } from '@/lib/utils';

function CvCard({ cv }: { cv: CvAnalysisListItemDto }) {
  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardHeader className="pb-2">
        <div className="flex items-start justify-between gap-2">
          <CardTitle className="text-sm font-medium truncate flex-1">
            {cv.originalFileName}
          </CardTitle>
          <StatusBadge status={cv.status} />
        </div>
      </CardHeader>
      <CardContent className="flex items-center justify-between">
        <span className="text-xs text-[var(--muted-foreground)]">
          {formatDate(cv.createdAt)}
        </span>
        <Button variant="outline" size="sm" asChild>
          <Link href={`/cv/${cv.id}`}>Analizi Gör</Link>
        </Button>
      </CardContent>
    </Card>
  );
}

function CvListSkeleton() {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {[1, 2, 3].map((i) => (
        <Card key={i}>
          <CardHeader className="pb-2">
            <Skeleton className="h-4 w-48" />
          </CardHeader>
          <CardContent className="flex justify-between">
            <Skeleton className="h-3 w-24" />
            <Skeleton className="h-8 w-20" />
          </CardContent>
        </Card>
      ))}
    </div>
  );
}

export default function CvListPage() {
  const [cvList, setCvList] = useState<CvAnalysisListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  async function fetchCvList() {
    setLoading(true);
    setError(null);
    try {
      const res = await apiClient.get<ApiResponse<CvAnalysisListItemDto[]>>('/api/cv/list');
      if (res.data.success) {
        setCvList(res.data.data);
      } else {
        setError(res.data.error ?? 'CV listesi alınamadı.');
      }
    } catch {
      setError('CV listesi yüklenirken hata oluştu.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchCvList();
  }, []);

  return (
    <div className="mx-auto max-w-7xl px-4 py-10">
      <div className="flex items-center justify-between mb-8">
        <h1 className="text-2xl font-bold">CV&apos;lerim</h1>
        <Button asChild>
          <Link href="/cv/upload">
            <Upload className="h-4 w-4 mr-2" />
            Yeni CV Yükle
          </Link>
        </Button>
      </div>

      {error && (
        <div className="mb-6">
          <ApiErrorAlert message={error} onRetry={fetchCvList} />
        </div>
      )}

      {loading ? (
        <CvListSkeleton />
      ) : cvList.length === 0 ? (
        <div className="flex flex-col items-center justify-center py-24 gap-4 text-center">
          <FileText className="h-12 w-12 text-[var(--muted-foreground)]" />
          <p className="text-lg font-medium">Henüz CV yüklemediniz</p>
          <p className="text-sm text-[var(--muted-foreground)]">
            İlk CV&apos;nizi yükleyerek kariyer analizine başlayın.
          </p>
          <Button asChild>
            <Link href="/cv/upload">CV Yükle</Link>
          </Button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {cvList.map((cv) => (
            <CvCard key={cv.id} cv={cv} />
          ))}
        </div>
      )}
    </div>
  );
}
