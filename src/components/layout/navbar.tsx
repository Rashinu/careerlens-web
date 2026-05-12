'use client';

import Link from 'next/link';
import { useTheme } from 'next-themes';
import { Moon, Sun, Menu, LayoutDashboard, FileText, BarChart2, LogOut } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from '@/components/ui/sheet';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { useAuth } from '@/context/auth-context';

function ThemeToggle() {
  const { theme, setTheme } = useTheme();
  return (
    <Button
      variant="ghost"
      size="icon"
      aria-label="Tema değiştir"
      onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}
    >
      <Sun className="h-4 w-4 rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0" />
      <Moon className="absolute h-4 w-4 rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100" />
    </Button>
  );
}

const navLinks = [
  { href: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { href: '/cv', label: "CV'lerim", icon: FileText },
  { href: '/salary', label: 'Maaş Karşılaştırma', icon: BarChart2 },
];

function UserInitials({ name }: { name: string }) {
  const initials = name
    .split(' ')
    .map((w) => w[0])
    .join('')
    .toUpperCase()
    .slice(0, 2);
  return (
    <span className="flex h-8 w-8 items-center justify-center rounded-full bg-[#2563EB] text-white text-sm font-semibold">
      {initials || '?'}
    </span>
  );
}

export function Navbar() {
  const { user, logout } = useAuth();
  const displayName = [user?.firstName, user?.lastName].filter(Boolean).join(' ') || user?.email || '';

  return (
    <header className="sticky top-0 z-40 border-b border-[var(--border)] bg-[var(--background)]/95 backdrop-blur">
      <nav className="mx-auto flex max-w-7xl items-center justify-between px-4 py-3">
        {/* Wordmark */}
        <Link href="/" className="flex items-center gap-1 text-lg font-bold">
          Career<span className="text-[#2563EB]">Lens</span>
          <span className="h-2 w-2 rounded-full bg-[#2563EB]" aria-hidden />
        </Link>

        {/* Desktop links */}
        <div className="hidden md:flex items-center gap-6">
          {user ? (
            <>
              {navLinks.map((link) => (
                <Link
                  key={link.href}
                  href={link.href}
                  className="text-sm font-medium text-[var(--muted-foreground)] hover:text-[var(--foreground)] transition-colors"
                >
                  {link.label}
                </Link>
              ))}
              <ThemeToggle />
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <button aria-label="Kullanıcı menüsü">
                    <UserInitials name={displayName} />
                  </button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <div className="px-2 py-1.5 text-sm font-medium">{displayName}</div>
                  <div className="px-2 pb-1.5 text-xs text-[var(--muted-foreground)]">{user.email}</div>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem onClick={logout} className="text-[var(--destructive)] cursor-pointer">
                    <LogOut className="h-4 w-4 mr-2" />
                    Çıkış Yap
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </>
          ) : (
            <>
              <ThemeToggle />
              <Button variant="ghost" asChild>
                <Link href="/login">Giriş Yap</Link>
              </Button>
              <Button asChild>
                <Link href="/register">Kayıt Ol</Link>
              </Button>
            </>
          )}
        </div>

        {/* Mobile hamburger */}
        <div className="flex md:hidden items-center gap-2">
          <ThemeToggle />
          <Sheet>
            <SheetTrigger asChild>
              <Button variant="ghost" size="icon" aria-label="Menüyü aç">
                <Menu className="h-5 w-5" />
              </Button>
            </SheetTrigger>
            <SheetContent side="right">
              <SheetHeader>
                <SheetTitle>
                  Career<span className="text-[#2563EB]">Lens</span>
                </SheetTitle>
              </SheetHeader>
              <div className="mt-6 flex flex-col gap-3">
                {user ? (
                  <>
                    {navLinks.map((link) => (
                      <Link
                        key={link.href}
                        href={link.href}
                        className="flex items-center gap-2 text-sm font-medium py-2 hover:text-[#2563EB] transition-colors"
                      >
                        <link.icon className="h-4 w-4" />
                        {link.label}
                      </Link>
                    ))}
                    <button
                      onClick={logout}
                      className="flex items-center gap-2 text-sm font-medium py-2 text-[var(--destructive)] hover:opacity-80 transition-opacity"
                    >
                      <LogOut className="h-4 w-4" />
                      Çıkış Yap
                    </button>
                  </>
                ) : (
                  <>
                    <Button variant="ghost" asChild className="justify-start">
                      <Link href="/login">Giriş Yap</Link>
                    </Button>
                    <Button asChild>
                      <Link href="/register">Kayıt Ol</Link>
                    </Button>
                  </>
                )}
              </div>
            </SheetContent>
          </Sheet>
        </div>
      </nav>
    </header>
  );
}
