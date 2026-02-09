'use client';

import Link from 'next/link';
import { RegisterForm } from '@/presentation/components/forms/RegisterForm';

export default function RegisterPage() {
  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6 text-center">Criar Conta</h1>
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <RegisterForm />
        <p className="mt-4 text-center text-sm text-gray-600">
          Já tem conta?{' '}
          <Link href="/login" className="text-blue-600 hover:underline">
            Faça login
          </Link>
        </p>
      </div>
    </div>
  );
}
