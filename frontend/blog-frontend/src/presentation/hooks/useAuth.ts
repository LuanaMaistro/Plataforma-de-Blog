'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { AuthRepository } from '@/infrastructure/repositories/AuthRepository';
import { LoginUseCase } from '@/application/use-cases/auth/loginUseCase';
import { RegisterUseCase } from '@/application/use-cases/auth/registerUseCase';
import { useAuthStore } from '@/presentation/stores/authStore';

export function useAuth() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const router = useRouter();
  const { setUser, logout: storeLogout } = useAuthStore();

  const repository = new AuthRepository();

  const login = async (email: string, senha: string) => {
    setLoading(true);
    setError(null);
    try {
      const useCase = new LoginUseCase(repository);
      const result = await useCase.execute({ email, senha });
      setUser({ id: result.id, nome: result.nome, email: result.email }, result.token);
      router.push('/posts');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Erro ao fazer login';
      setError(message);
    } finally {
      setLoading(false);
    }
  };

  const register = async (nome: string, email: string, senha: string) => {
    setLoading(true);
    setError(null);
    try {
      const useCase = new RegisterUseCase(repository);
      await useCase.execute({ nome, email, senha });
      router.push('/login');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Erro ao registrar';
      setError(message);
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    storeLogout();
    router.push('/login');
  };

  return { login, register, logout, loading, error };
}
