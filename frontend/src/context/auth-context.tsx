'use client';

import React, { createContext, useContext, useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import apiClient from '@/lib/api-client';
import type { UserProfileDto } from '@/types/api';

interface AuthContextValue {
  user: UserProfileDto | null;
  loading: boolean;
  login: (accessToken: string, refreshToken: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<UserProfileDto | null>(null);
  const [loading, setLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    const token =
      typeof window !== 'undefined'
        ? localStorage.getItem('cl_access_token')
        : null;
    if (!token) {
      setLoading(false);
      return;
    }
    apiClient
      .get<{ success: boolean; data: UserProfileDto }>('/api/users/me')
      .then((res) => {
        if (res.data.success) setUser(res.data.data);
      })
      .catch(() => {
        localStorage.removeItem('cl_access_token');
        localStorage.removeItem('cl_refresh_token');
      })
      .finally(() => setLoading(false));
  }, []);

  async function login(accessToken: string, refreshToken: string) {
    localStorage.setItem('cl_access_token', accessToken);
    localStorage.setItem('cl_refresh_token', refreshToken);
    // Also set cookie so middleware can gate protected routes
    document.cookie = `cl_access_token=${accessToken}; path=/; SameSite=Lax`;
    const res = await apiClient.get<{ success: boolean; data: UserProfileDto }>(
      '/api/users/me'
    );
    if (res.data.success) setUser(res.data.data);
  }

  function logout() {
    localStorage.removeItem('cl_access_token');
    localStorage.removeItem('cl_refresh_token');
    document.cookie = 'cl_access_token=; path=/; max-age=0';
    setUser(null);
    router.push('/login');
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}
