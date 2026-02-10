'use client';

import { useState } from 'react';
import { usePosts } from '@/presentation/hooks/usePosts';
import { useAuthStore } from '@/presentation/stores/authStore';
import { PostCard } from '@/presentation/components/ui/PostCard';
import { Loading } from '@/presentation/components/ui/Loading';
import { ErrorMessage } from '@/presentation/components/ui/ErrorMessage';
import { ConfirmModal } from '@/presentation/components/ui/ConfirmModal';

export default function PostsPage() {
  const { posts, loading, error, deletePost } = usePosts();
  const { user } = useAuthStore();
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
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Todas as Postagens</h1>
      {posts.length === 0 ? (
        <p className="text-gray-500 text-center py-12">Nenhuma postagem encontrada.</p>
      ) : (
        <div className="flex flex-col gap-4">
          {posts.map((post) => (
            <PostCard
              key={post.id}
              post={post}
              showActions={user?.id === post.autor.id}
              onDelete={(id) => setDeleteId(id)}
            />
          ))}
        </div>
      )}

      {deleteId !== null && (
        <ConfirmModal
          title="Excluir postagem"
          message="Tem certeza que deseja excluir esta postagem? Esta ação não pode ser desfeita."
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
