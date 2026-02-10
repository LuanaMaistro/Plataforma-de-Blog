'use client';

import Link from 'next/link';
import { LoginForm } from '@/presentation/components/forms/LoginForm';

export default function LoginPage() {
  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6 text-center">Login</h1>
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <LoginForm />
        <p className="mt-4 text-center text-sm text-gray-600">
          Não tem conta?{' '}
          <Link href="/register" className="text-primary hover:underline">
            Registre-se
          </Link>
        </p>
      </div>
    </div>
  );
}
