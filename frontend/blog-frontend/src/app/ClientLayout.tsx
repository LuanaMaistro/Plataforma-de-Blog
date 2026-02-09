'use client';

import { useEffect } from 'react';
import { Header } from '@/presentation/components/layout/Header';
import { useAuthStore } from '@/presentation/stores/authStore';

export function ClientLayout({ children }: { children: React.ReactNode }) {
  const loadFromStorage = useAuthStore((s) => s.loadFromStorage);

  useEffect(() => {
    loadFromStorage();
  }, [loadFromStorage]);

  return (
    <>
      <Header />
      <main className="max-w-5xl mx-auto px-4 py-8">
        {children}
      </main>
    </>
  );
}
