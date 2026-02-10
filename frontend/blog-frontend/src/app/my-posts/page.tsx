'use client';

import Link from 'next/link';
import { usePosts } from '@/presentation/hooks/usePosts';
import { PostCard } from '@/presentation/components/ui/PostCard';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';

export default function MyPostsPage() {
  const { posts, loading, error, deletePost } = usePosts(true);

  const handleDelete = async (id: number) => {
    if (!confirm('Tem certeza que deseja excluir esta postagem?')) return;
    try {
      await deletePost(id);
    } catch {
      alert('Erro ao excluir postagem');
    }
  };

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-gray-900">Minhas Postagens</h1>
        <Link
          href="/posts/new"
          className="bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark transition-colors"
        >
          Nova Postagem
        </Link>
      </div>
      {posts.length === 0 ? (
        <p className="text-gray-500 text-center py-12">
          Você ainda não tem postagens.
        </p>
      ) : (
        <div className="flex flex-col gap-4">
          {posts.map((post) => (
            <PostCard
              key={post.id}
              post={post}
              showActions
              onDelete={handleDelete}
            />
          ))}
        </div>
      )}
    </div>
  );
}
