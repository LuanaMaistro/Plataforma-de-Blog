'use client';

import Link from 'next/link';
import { useAuthStore } from '@/presentation/stores/authStore';

export default function HomePage() {
  const { isAuthenticated } = useAuthStore();

  return (
    <div className="flex flex-col items-center justify-center py-20 text-center">
      <h1 className="text-4xl font-bold text-gray-900 mb-4">
        Bem-vindo ao Blog
      </h1>
      <p className="text-lg text-gray-600 mb-8 max-w-md">
        Uma plataforma para compartilhar ideias e conhecimento.
      </p>
      <div className="flex gap-4">
        <Link
          href="/posts"
          className="bg-primary text-white px-6 py-3 rounded-lg hover:bg-primary-dark transition-colors"
        >
          Ver Postagens
        </Link>
        {!isAuthenticated && (
          <Link
            href="/register"
            className="bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition-colors"
          >
            Criar Conta
          </Link>
        )}
      </div>
    </div>
  );
}
