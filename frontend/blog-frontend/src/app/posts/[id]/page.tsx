'use client';

import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import { Postagem } from '@/domain/entities/Postagem';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { GetPostUseCase } from '@/application/use-cases/posts/getPostUseCase';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';

export default function PostDetailPage() {
  const params = useParams();
  const [post, setPost] = useState<Postagem | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const id = Number(params.id);
    const repository = new PostRepository();
    const useCase = new GetPostUseCase(repository);

    useCase.execute(id)
      .then(setPost)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [params.id]);

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;
  if (!post) return <ErrorMessage message="Postagem não encontrada" />;

  const dataFormatada = new Date(post.dataCriacao).toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: 'long',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });

  return (
    <div className="max-w-3xl mx-auto">
      <article className="bg-white p-8 rounded-lg shadow-sm border">
        <h1 className="text-3xl font-bold text-gray-900 mb-4">{post.titulo}</h1>
        <div className="text-sm text-gray-500 mb-8">
          Por {post.autor.nome} em {dataFormatada}
        </div>
        <div className="text-gray-700 leading-relaxed whitespace-pre-wrap">
          {post.conteudo}
        </div>
      </article>
    </div>
  );
}
