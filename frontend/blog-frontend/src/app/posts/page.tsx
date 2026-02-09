'use client';

import { usePosts } from '@/presentation/hooks/usePosts';
import { PostCard } from '@/presentation/components/ui/PostCard';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';

export default function PostsPage() {
  const { posts, loading, error } = usePosts();

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Todas as Postagens</h1>
      {posts.length === 0 ? (
        <p className="text-gray-500 text-center py-12">Nenhuma postagem encontrada.</p>
      ) : (
        <div className="flex flex-col gap-4">
          {posts.map((post) => (
            <PostCard key={post.id} post={post} />
          ))}
        </div>
      )}
    </div>
  );
}
