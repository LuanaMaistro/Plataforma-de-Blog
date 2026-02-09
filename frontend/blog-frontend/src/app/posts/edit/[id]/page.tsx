'use client';

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { Postagem } from '@/domain/entities/Postagem';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { GetPostUseCase } from '@/application/use-cases/posts/getPostUseCase';
import { UpdatePostUseCase } from '@/application/use-cases/posts/updatePostUseCase';
import { PostForm } from '@/presentation/components/forms/PostForm';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';

export default function EditPostPage() {
  const params = useParams();
  const router = useRouter();
  const [post, setPost] = useState<Postagem | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const repository = new PostRepository();

  useEffect(() => {
    const id = Number(params.id);
    const getUseCase = new GetPostUseCase(repository);

    getUseCase.execute(id)
      .then(setPost)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [params.id]);

  const handleSubmit = async (titulo: string, conteudo: string) => {
    const id = Number(params.id);
    const useCase = new UpdatePostUseCase(repository);
    await useCase.execute(id, { titulo, conteudo });
    router.push('/my-posts');
  };

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;
  if (!post) return <ErrorMessage message="Postagem não encontrada" />;

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Editar Postagem</h1>
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <PostForm
          initialTitulo={post.titulo}
          initialConteudo={post.conteudo}
          onSubmit={handleSubmit}
          submitLabel="Salvar"
        />
      </div>
    </div>
  );
}
