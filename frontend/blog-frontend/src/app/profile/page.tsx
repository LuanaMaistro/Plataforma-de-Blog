'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { Input } from '@/presentation/components/ui/Input';
import { Button } from '@/presentation/components/ui/Button';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';
import { useAuthStore } from '@/presentation/stores/authStore';
import { AuthRepository } from '@/infrastructure/repositories/AuthRepository';

export default function ProfilePage() {
  const { user, setUser } = useAuthStore();
  const router = useRouter();

  const [nome, setNome] = useState(user?.nome ?? '');
  const [senhaAtual, setSenhaAtual] = useState('');
  const [novaSenha, setNovaSenha] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    if (!user) {
      router.push('/login');
    }
  }, [user, router]);

  if (!user) {
    return null;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);

    if (nome.length < 3) {
      setError('Nome deve ter pelo menos 3 caracteres');
      return;
    }

    if (novaSenha && !senhaAtual) {
      setError('Informe a senha atual para alterar a senha');
      return;
    }

    if (novaSenha && novaSenha.length < 6) {
      setError('Nova senha deve ter pelo menos 6 caracteres');
      return;
    }

    if (novaSenha && !/[A-Z]/.test(novaSenha)) {
      setError('Nova senha deve conter pelo menos uma letra maiúscula');
      return;
    }

    if (novaSenha && !/[0-9]/.test(novaSenha)) {
      setError('Nova senha deve conter pelo menos um número');
      return;
    }

    setLoading(true);
    try {
      const repository = new AuthRepository();
      const updated = await repository.atualizarPerfil(
        nome,
        senhaAtual || undefined,
        novaSenha || undefined
      );

      const token = localStorage.getItem('token')!;
      setUser({ id: updated.id, nome: updated.nome, email: updated.email }, token);

      setSenhaAtual('');
      setNovaSenha('');
      setSuccess('Perfil atualizado com sucesso!');
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Erro ao atualizar perfil';
      setError(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6 text-center">Meu Perfil</h1>
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          {error && <ErrorMessage message={error} />}
          {success && (
            <div className="bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-lg">
              {success}
            </div>
          )}

          <Input
            label="Email"
            type="email"
            value={user.email}
            disabled
            className="bg-gray-100"
          />

          <Input
            label="Nome"
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            placeholder="Seu nome"
          />

          <hr className="my-2" />

          <Input
            label="Senha atual"
            type="password"
            value={senhaAtual}
            onChange={(e) => setSenhaAtual(e.target.value)}
            placeholder="Senha atual"
          />

          <Input
            label="Nova senha"
            type="password"
            value={novaSenha}
            onChange={(e) => setNovaSenha(e.target.value)}
            placeholder="Nova senha (min. 6 caracteres)"
          />

          <Button type="submit" loading={loading}>
            Salvar Alterações
          </Button>
        </form>
      </div>
    </div>
  );
}
