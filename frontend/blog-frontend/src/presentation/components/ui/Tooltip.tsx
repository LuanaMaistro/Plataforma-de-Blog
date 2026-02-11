'use client';

import { ReactNode, useState } from 'react';

interface TooltipProps {
  children: ReactNode;
  label: string;
}

export function Tooltip({ children, label }: TooltipProps) {
  const [visible, setVisible] = useState(false);

  return (
    <div
      className="relative inline-flex"
      onMouseEnter={() => setVisible(true)}
      onMouseLeave={() => setVisible(false)}
    >
      {children}
      <span
        className={`absolute bottom-full left-1/2 -translate-x-1/2 mb-2 px-2.5 py-1 text-sm font-medium text-gray-600 bg-white border border-gray-200 rounded-md shadow-sm whitespace-nowrap pointer-events-none transition-all duration-200 ${
          visible ? 'opacity-100 translate-y-0' : 'opacity-0 translate-y-1'
        }`}
      >
        {label}
        <span className="absolute top-full left-1/2 -translate-x-1/2 border-4 border-transparent border-t-gray-200" />
      </span>
    </div>
  );
}
