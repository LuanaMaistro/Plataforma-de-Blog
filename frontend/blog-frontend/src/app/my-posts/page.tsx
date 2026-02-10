'use client';

import { useState } from 'react';
import Link from 'next/link';
import { usePosts } from '@/presentation/hooks/usePosts';
import { PostCard } from '@/presentation/components/ui/PostCard';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';
import { ConfirmModal } from '@/presentation/components/ui/ConfirmModal';

export default function MyPostsPage() {
  const { posts, loading, error, deletePost } = usePosts(true);
  const [deleteId, setDeleteId] = useState<number | null>(null);

  const handleDelete = async () => {
    if (deleteId === null) return;
    try {
      await deletePost(deleteId);
    } catch {
      alert('Erro ao excluir postagem');
    }
    setDeleteId(null);
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
              onDelete={(id) => setDeleteId(id)}
            />
          ))}
        </div>
      )}

      {deleteId !== null && (
        <ConfirmModal
          title="Excluir postagem"
          message="Tem certeza que deseja excluir a postagem? Esta ação não pode ser desfeita."
          confirmLabel="Excluir"
          cancelLabel="Cancelar"
          variant="danger"
          onConfirm={handleDelete}
          onCancel={() => setDeleteId(null)}
        />
      )}
    </div>
  );
}
