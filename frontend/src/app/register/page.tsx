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

interface RegisterForm {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export default function RegisterPage() {
  const { login } = useAuth();
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<RegisterForm>();

  async function onSubmit(data: RegisterForm) {
    setIsLoading(true);
    try {
      const res = await apiClient.post<ApiResponse<AuthResponse>>(
        '/api/auth/register',
        {
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          password: data.password,
        }
      );
      if (res.data.success) {
        await login(res.data.data.accessToken, res.data.data.refreshToken);
        toast.success('Hesabınız oluşturuldu!');
        router.push('/dashboard');
      } else {
        toast.error(res.data.error ?? 'Kayıt başarısız.');
      }
    } catch {
      toast.error('Kayıt sırasında bir hata oluştu.');
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-4rem)] items-center justify-center px-4 py-12">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <CardTitle className="text-2xl">Kayıt Ol</CardTitle>
          <CardDescription>Ücretsiz hesabınızı oluşturun</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
            <div className="grid grid-cols-2 gap-3">
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="firstName">Ad</Label>
                <Input
                  id="firstName"
                  placeholder="Adınız"
                  {...register('firstName', { required: 'Ad zorunludur.' })}
                />
                {errors.firstName && (
                  <p className="text-xs text-[var(--destructive)]">{errors.firstName.message}</p>
                )}
              </div>
              <div className="flex flex-col gap-1.5">
                <Label htmlFor="lastName">Soyad</Label>
                <Input
                  id="lastName"
                  placeholder="Soyadınız"
                  {...register('lastName', { required: 'Soyad zorunludur.' })}
                />
                {errors.lastName && (
                  <p className="text-xs text-[var(--destructive)]">{errors.lastName.message}</p>
                )}
              </div>
            </div>
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
                placeholder="En az 8 karakter"
                autoComplete="new-password"
                {...register('password', {
                  required: 'Şifre zorunludur.',
                  minLength: { value: 8, message: 'Şifre en az 8 karakter olmalı.' },
                })}
              />
              {errors.password && (
                <p className="text-xs text-[var(--destructive)]">{errors.password.message}</p>
              )}
            </div>
            <div className="flex flex-col gap-1.5">
              <Label htmlFor="confirmPassword">Şifre Tekrar</Label>
              <Input
                id="confirmPassword"
                type="password"
                placeholder="Şifrenizi tekrar girin"
                autoComplete="new-password"
                {...register('confirmPassword', {
                  required: 'Şifre tekrarı zorunludur.',
                  validate: (val) =>
                    val === watch('password') || 'Şifreler eşleşmiyor.',
                })}
              />
              {errors.confirmPassword && (
                <p className="text-xs text-[var(--destructive)]">{errors.confirmPassword.message}</p>
              )}
            </div>
            <Button type="submit" className="w-full mt-2" disabled={isLoading}>
              {isLoading ? 'Hesap oluşturuluyor...' : 'Kayıt Ol'}
            </Button>
          </form>
          <p className="mt-4 text-center text-sm text-[var(--muted-foreground)]">
            Zaten hesabınız var mı?{' '}
            <Link href="/login" className="text-[#2563EB] hover:underline font-medium">
              Giriş Yap
            </Link>
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
