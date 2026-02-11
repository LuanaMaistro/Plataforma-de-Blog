'use client';

import { ReactNode } from 'react';

interface TimelineProps {
  children: ReactNode;
}

export function Timeline({ children }: TimelineProps) {
  return (
    <div className="relative flex flex-col gap-8 pl-8">
      <div className="absolute left-3 top-3 bottom-3 w-0.5 bg-primary/30" />
      {children}
    </div>
  );
}
