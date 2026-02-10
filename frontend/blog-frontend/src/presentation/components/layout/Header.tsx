'use client';

import { useState } from 'react';
import Link from 'next/link';
import { useAuthStore } from '@/presentation/stores/authStore';
import { useAuth } from '@/presentation/hooks/useAuth';
import { ConfirmModal } from '@/presentation/components/ui/ConfirmModal';

export function Header() {
  const { isAuthenticated, user } = useAuthStore();
  const { logout } = useAuth();
  const [showLogoutModal, setShowLogoutModal] = useState(false);

  return (
    <>
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-5xl mx-auto px-4 py-4 flex items-center justify-between">
          <Link href="/" className="text-xl font-bold text-primary">
            Blog
          </Link>

          <nav className="flex items-center gap-4">
            <Link href="/posts" className="text-gray-600 hover:text-gray-900">
              Postagens
            </Link>

            {isAuthenticated ? (
              <>
                <Link href="/my-posts" className="text-gray-600 hover:text-gray-900">
                  Minhas Postagens
                </Link>
                <Link href="/posts/new" className="text-gray-600 hover:text-gray-900">
                  Nova Postagem
                </Link>
                <Link href="/profile" className="text-sm text-primary hover:text-primary-dark font-medium">
                  {user?.nome}
                </Link>
                <button
                  onClick={() => setShowLogoutModal(true)}
                  className="text-sm text-red-600 hover:text-red-800"
                >
                  Sair
                </button>
              </>
            ) : (
              <>
                <Link href="/login" className="text-gray-600 hover:text-gray-900">
                  Login
                </Link>
                <Link href="/register" className="bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark">
                  Registrar
                </Link>
              </>
            )}
          </nav>
        </div>
      </header>

      {showLogoutModal && (
        <ConfirmModal
          title="Sair do sistema"
          message="Tem certeza que deseja sair?"
          confirmLabel="Sair"
          cancelLabel="Cancelar"
          variant="danger"
          onConfirm={logout}
          onCancel={() => setShowLogoutModal(false)}
        />
      )}
    </>
  );
}
