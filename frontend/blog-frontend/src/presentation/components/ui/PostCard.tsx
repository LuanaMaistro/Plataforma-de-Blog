'use client';

import Link from 'next/link';
import { Postagem } from '@/domain/entities/Postagem';

interface PostCardProps {
  post: Postagem;
  showActions?: boolean;
  onDelete?: (id: number) => void;
}

export function PostCard({ post, showActions, onDelete }: PostCardProps) {
  const resumo = post.conteudo.length > 150
    ? post.conteudo.substring(0, 150) + '...'
    : post.conteudo;

  const dataFormatada = new Date(post.dataCriacao).toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: 'long',
    year: 'numeric',
  });

  return (
    <div className="bg-white border rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
      <Link href={`/posts/${post.id}`}>
        <h2 className="text-xl font-semibold text-gray-900 hover:text-blue-600 mb-2">
          {post.titulo}
        </h2>
      </Link>
      <p className="text-gray-600 mb-4">{resumo}</p>
      <div className="flex items-center justify-between text-sm text-gray-500">
        <span>Por {post.autor.nome} em {dataFormatada}</span>
        {showActions && (
          <div className="flex gap-2">
            <Link
              href={`/posts/edit/${post.id}`}
              className="text-blue-600 hover:text-blue-800"
            >
              Editar
            </Link>
            <button
              onClick={() => onDelete?.(post.id)}
              className="text-red-600 hover:text-red-800"
            >
              Excluir
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
