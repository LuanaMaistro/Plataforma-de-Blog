'use client';

import { useState } from 'react';
import { Input } from '@/presentation/components/ui/Input';
import { TextArea } from '@/presentation/components/ui/TextArea';
import { Button } from '@/presentation/components/ui/Button';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';
import { postValidator } from '@/application/validators/postValidator';

interface PostFormProps {
  initialTitulo?: string;
  initialConteudo?: string;
  onSubmit: (titulo: string, conteudo: string) => Promise<void>;
  submitLabel: string;
}

export function PostForm({ initialTitulo = '', initialConteudo = '', onSubmit, submitLabel }: PostFormProps) {
  const [titulo, setTitulo] = useState(initialTitulo);
  const [conteudo, setConteudo] = useState(initialConteudo);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setFieldErrors({});
    setError(null);

    const validation = postValidator.safeParse({ titulo, conteudo });
    if (!validation.success) {
      const errors: Record<string, string> = {};
      validation.error.issues.forEach((err) => {
        if (err.path[0]) errors[err.path[0] as string] = err.message;
      });
      setFieldErrors(errors);
      return;
    }

    setLoading(true);
    try {
      await onSubmit(titulo, conteudo);
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Erro ao salvar postagem';
      setError(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
      {error && <ErrorMessage message={error} />}
      <Input
        label="Título"
        value={titulo}
        onChange={(e) => setTitulo(e.target.value)}
        placeholder="Título da postagem"
        error={fieldErrors.titulo}
      />
      <TextArea
        label="Conteúdo"
        value={conteudo}
        onChange={(e) => setConteudo(e.target.value)}
        placeholder="Escreva o conteúdo da postagem..."
        error={fieldErrors.conteudo}
      />
      <Button type="submit" loading={loading}>
        {submitLabel}
      </Button>
    </form>
  );
}
