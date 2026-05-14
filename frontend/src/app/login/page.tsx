'use client';

import { useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { useAuth } from '@/context/auth-context';
import apiClient from '@/lib/api-client';
import type { ApiResponse, AuthResponse } from '@/types/api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';

interface LoginForm {
  email: string;
  password: string;
}

export default function LoginPage() {
  const { login } = useAuth();
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>();

  async function onSubmit(data: LoginForm) {
    setIsLoading(true);
    try {
      const res = await apiClient.post<ApiResponse<AuthResponse>>(
        '/api/auth/login',
        data
      );
      if (res.data.success) {
        await login(res.data.data.accessToken, res.data.data.refreshToken);
        toast.success('Giriş başarılı!');
        router.push('/dashboard');
      } else {
        toast.error(res.data.error ?? 'Giriş başarısız.');
      }
    } catch {
      toast.error('E-posta veya şifre hatalı.');
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-4rem)] items-center justify-center px-4 py-12">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <CardTitle className="text-2xl">Giriş Yap</CardTitle>
          <CardDescription>
            Hesabınıza giriş yapın
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="email">E-posta</Label>
              <Input
                id="email"
                type="email"
                placeholder="ornek@sirket.com"
                autoComplete="email"
                {...register('email', {
                  required: 'E-posta zorunludur.',
                  pattern: { value: /\S+@\S+\.\S+/, message: 'Geçerli bir e-posta girin.' },
                })}
              />
              {errors.email && (
                <p className="text-xs text-[var(--destructive)]">{errors.email.message}</p>
              )}
            </div>
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="password">Şifre</Label>
              <Input
                id="password"
                type="password"
                placeholder="Şifreniz"
                autoComplete="current-password"
                {...register('password', { required: 'Şifre zorunludur.' })}
              />
              {errors.password && (
                <p className="text-xs text-[var(--destructive)]">{errors.password.message}</p>
              )}
            </div>
            <Button type="submit" className="w-full mt-2" disabled={isLoading}>
              {isLoading ? 'Giriş yapılıyor...' : 'Giriş Yap'}
            </Button>
          </form>
          <p className="mt-4 text-center text-sm text-[var(--muted-foreground)]">
            Hesabınız yok mu?{' '}
            <Link href="/register" className="text-[#2563EB] hover:underline font-medium">
              Kayıt Ol
            </Link>
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
