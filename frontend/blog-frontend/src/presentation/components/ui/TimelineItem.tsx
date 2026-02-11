'use client';

import { ReactNode } from 'react';
import { Calendar } from 'lucide-react';

interface TimelineItemProps {
  date: string;
  children: ReactNode;
}

export function TimelineItem({ date, children }: TimelineItemProps) {
  const dataFormatada = new Date(date).toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
  });

  return (
    <div className="relative">
      <div className="absolute -left-8 top-6 flex items-center justify-center">
        <div className="w-6 h-6 rounded-full bg-primary border-4 border-white shadow-sm" />
      </div>

      <div className="flex items-center gap-1.5 mb-2 text-sm font-medium text-primary-dark">
        <Calendar size={14} />
        <span>{dataFormatada}</span>
      </div>

      {children}
    </div>
  );
}
