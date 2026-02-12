'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { PostForm } from '@/presentation/components/forms/PostForm';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { CreatePostUseCase } from '@/application/use-cases/posts/createPostUseCase';

export default function NewPostPage() {
  const router = useRouter();
  const repository = new PostRepository();
  const [success, setSuccess] = useState<string | null>(null);

  const handleSubmit = async (titulo: string, conteudo: string) => {
    const useCase = new CreatePostUseCase(repository);
    await useCase.execute({ titulo, conteudo });
    setSuccess('Postagem publicada com sucesso!');
    setTimeout(() => router.push('/my-posts'), 1500);
  };

  return (
    <div className="max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Nova Postagem</h1>
      {success && (
        <div className="bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-lg mb-4">
          {success}
        </div>
      )}
      <div className="bg-white p-6 rounded-lg shadow-sm border">
        <PostForm onSubmit={handleSubmit} submitLabel="Publicar" />
      </div>
    </div>
  );
}
