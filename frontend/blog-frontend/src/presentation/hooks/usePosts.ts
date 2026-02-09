'use client';

import { useState, useEffect, useCallback } from 'react';
import { Postagem } from '@/domain/entities/Postagem';
import { PostRepository } from '@/infrastructure/repositories/PostRepository';
import { ListPostsUseCase } from '@/application/use-cases/posts/listPostsUseCase';
import { CreatePostUseCase } from '@/application/use-cases/posts/createPostUseCase';
import { UpdatePostUseCase } from '@/application/use-cases/posts/updatePostUseCase';
import { DeletePostUseCase } from '@/application/use-cases/posts/deletePostUseCase';

export function usePosts(apenasMinhas = false) {
  const [posts, setPosts] = useState<Postagem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const repository = new PostRepository();

  const fetchPosts = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const useCase = new ListPostsUseCase(repository);
      const result = await useCase.execute(apenasMinhas);
      setPosts(result);
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : 'Erro ao carregar postagens';
      setError(message);
    } finally {
      setLoading(false);
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [apenasMinhas]);

  useEffect(() => {
    fetchPosts();
  }, [fetchPosts]);

  const createPost = async (titulo: string, conteudo: string) => {
    const useCase = new CreatePostUseCase(repository);
    const post = await useCase.execute({ titulo, conteudo });
    setPosts((prev) => [post, ...prev]);
    return post;
  };

  const updatePost = async (id: number, titulo: string, conteudo: string) => {
    const useCase = new UpdatePostUseCase(repository);
    const post = await useCase.execute(id, { titulo, conteudo });
    setPosts((prev) => prev.map((p) => (p.id === id ? post : p)));
    return post;
  };

  const deletePost = async (id: number) => {
    const useCase = new DeletePostUseCase(repository);
    await useCase.execute(id);
    setPosts((prev) => prev.filter((p) => p.id !== id));
  };

  return { posts, loading, error, fetchPosts, createPost, updatePost, deletePost };
}
