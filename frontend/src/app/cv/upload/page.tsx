'use client';

import { useCallback, useState } from 'react';
import { useRouter } from 'next/navigation';
import { UploadCloud, File, X, CheckCircle, Loader2 } from 'lucide-react';
import { toast } from 'sonner';
import apiClient from '@/lib/api-client';
import type { ApiResponse, CvAnalysisDto } from '@/types/api';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useStatusPolling } from '@/hooks/use-status-polling';
import { getStatusLabel } from '@/lib/utils';

const PIPELINE_STEPS = ['Uploaded', 'TextExtracted', 'Analyzed'];

function PipelineStepper({ currentStatus }: { currentStatus: string | null }) {
  return (
    <div className="flex items-center gap-2 mt-6">
      {PIPELINE_STEPS.map((step, idx) => {
        const stepIndex = PIPELINE_STEPS.indexOf(currentStatus ?? '');
        const isDone = stepIndex > idx || currentStatus === step;
        const isActive = currentStatus === step;
        return (
          <div key={step} className="flex items-center gap-2">
            <div
              className={`flex h-8 w-8 items-center justify-center rounded-full text-xs font-bold transition-colors ${
                isDone
                  ? 'bg-green-500 text-white'
                  : isActive
                  ? 'bg-[#2563EB] text-white'
                  : 'bg-[var(--muted)] text-[var(--muted-foreground)]'
              }`}
            >
              {isDone && !isActive ? <CheckCircle className="h-4 w-4" /> : idx + 1}
            </div>
            <span
              className={`text-xs hidden sm:inline ${isActive ? 'font-semibold' : 'text-[var(--muted-foreground)]'}`}
            >
              {getStatusLabel(step)}
            </span>
            {idx < PIPELINE_STEPS.length - 1 && (
              <div className="h-px w-6 bg-[var(--border)]" />
            )}
          </div>
        );
      })}
    </div>
  );
}

export default function CvUploadPage() {
  const router = useRouter();
  const [file, setFile] = useState<File | null>(null);
  const [dragging, setDragging] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [uploadedId, setUploadedId] = useState<string | null>(null);

  const { status } = useStatusPolling(uploadedId, 3000);

  const handleDrop = useCallback((e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    setDragging(false);
    const dropped = e.dataTransfer.files[0];
    if (dropped) validateAndSet(dropped);
  }, []);

  function validateAndSet(f: File) {
    const allowed = ['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
    if (!allowed.includes(f.type)) {
      toast.error('Yalnızca PDF veya DOCX dosyaları desteklenir.');
      return;
    }
    if (f.size > 5 * 1024 * 1024) {
      toast.error('Dosya boyutu 5 MB\'ı geçemez.');
      return;
    }
    setFile(f);
  }

  async function handleUpload() {
    if (!file) return;
    setUploading(true);
    try {
      const formData = new FormData();
      formData.append('file', file);
      const res = await apiClient.post<ApiResponse<CvAnalysisDto>>(
        '/api/cv/upload',
        formData,
        { headers: { 'Content-Type': 'multipart/form-data' } }
      );
      if (res.data.success) {
        setUploadedId(res.data.data.id);
        toast.success('CV başarıyla yüklendi. Analiz başlatıldı.');
      } else {
        toast.error(res.data.error ?? 'Yükleme başarısız.');
      }
    } catch {
      toast.error('CV yüklenirken hata oluştu.');
    } finally {
      setUploading(false);
    }
  }

  if (status === 'Analyzed' && uploadedId) {
    router.push(`/cv/${uploadedId}`);
    return null;
  }

  return (
    <div className="mx-auto max-w-2xl px-4 py-16">
      <h1 className="text-2xl font-bold mb-8 text-center">CV Yükle</h1>

      {!uploadedId ? (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Dosya Seç</CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col gap-4">
            {/* Drop zone */}
            <div
              role="button"
              tabIndex={0}
              aria-label="CV dosyası yükle"
              onDragOver={(e) => { e.preventDefault(); setDragging(true); }}
              onDragLeave={() => setDragging(false)}
              onDrop={handleDrop}
              onClick={() => document.getElementById('cv-file-input')?.click()}
              onKeyDown={(e) => e.key === 'Enter' && document.getElementById('cv-file-input')?.click()}
              className={`flex flex-col items-center justify-center gap-3 rounded-lg border-2 border-dashed p-12 cursor-pointer transition-colors ${
                dragging
                  ? 'border-[#2563EB] bg-blue-50 dark:bg-blue-950'
                  : 'border-[var(--border)] hover:border-[#2563EB]'
              }`}
            >
              <UploadCloud className="h-10 w-10 text-[var(--muted-foreground)]" />
              <p className="text-sm font-medium">Sürükle & Bırak veya tıkla</p>
              <p className="text-xs text-[var(--muted-foreground)]">PDF, DOCX — maks. 5 MB</p>
              <input
                id="cv-file-input"
                type="file"
                accept=".pdf,.docx"
                className="hidden"
                onChange={(e) => {
                  const f = e.target.files?.[0];
                  if (f) validateAndSet(f);
                }}
              />
            </div>

            {/* Preview row */}
            {file && (
              <div className="flex items-center gap-3 rounded-lg border border-[var(--border)] p-3">
                <File className="h-5 w-5 text-[#2563EB] shrink-0" />
                <span className="text-sm flex-1 truncate">{file.name}</span>
                <span className="text-xs text-[var(--muted-foreground)] shrink-0">
                  {(file.size / 1024 / 1024).toFixed(2)} MB
                </span>
                <button
                  onClick={() => setFile(null)}
                  aria-label="Dosyayı kaldır"
                  className="text-[var(--muted-foreground)] hover:text-[var(--destructive)]"
                >
                  <X className="h-4 w-4" />
                </button>
              </div>
            )}

            <Button
              onClick={handleUpload}
              disabled={!file || uploading}
              className="w-full"
            >
              {uploading ? (
                <>
                  <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                  Yükleniyor...
                </>
              ) : (
                'CV Yükle ve Analizi Başlat'
              )}
            </Button>
          </CardContent>
        </Card>
      ) : (
        <Card>
          <CardHeader>
            <CardTitle className="text-base">Analiz Devam Ediyor</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-[var(--muted-foreground)] mb-2">
              CV&apos;niz işleniyor, lütfen bekleyin...
            </p>
            <PipelineStepper currentStatus={status} />
            {status === 'Failed' && (
              <p className="mt-4 text-sm text-[var(--destructive)]">
                Analiz başarısız oldu. Lütfen tekrar deneyin.
              </p>
            )}
          </CardContent>
        </Card>
      )}
    </div>
  );
}
