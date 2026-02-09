'use client';

import { useRouter } from 'next/navigation';
import { PostForm } from '@/presentation/components/forms/PostForm';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { CreatePostUseCase } from '@/application/use-cases/posts/createPostUseCase';

export default function NewPostPage() {
  const router = useRouter();
  const repository = new PostRepository();

  const handleSubmit = async (titulo: string, conteudo: string) => {
    const useCase = new CreatePostUseCase(repository);
    await useCase.execute({ titulo, conteudo });
    router.push('/my-posts');
  };

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Nova Postagem</h1>
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <PostForm onSubmit={handleSubmit} submitLabel="Publicar" />
      </div>
    </div>
  );
}
