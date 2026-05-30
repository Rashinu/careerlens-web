import { ImageResponse } from 'next/og';

export const runtime = 'edge';
export const alt = 'CareerLens — AI Destekli Kariyer Analizi';
export const size = { width: 1200, height: 630 };
export const contentType = 'image/png';

export default function OgImage() {
  return new ImageResponse(
    (
      <div
        style={{
          background: '#09090b',
          width: '100%',
          height: '100%',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          padding: '80px',
          fontFamily: 'system-ui, sans-serif',
          position: 'relative',
          overflow: 'hidden',
        }}
      >
        {/* Background glow */}
        <div style={{
          position: 'absolute', top: -100, left: -100,
          width: 500, height: 500, borderRadius: '50%',
          background: 'radial-gradient(circle, rgba(37,99,235,0.25) 0%, transparent 70%)',
        }} />
        <div style={{
          position: 'absolute', bottom: -100, right: -100,
          width: 400, height: 400, borderRadius: '50%',
          background: 'radial-gradient(circle, rgba(124,58,237,0.2) 0%, transparent 70%)',
        }} />

        {/* Logo */}
        <div style={{ display: 'flex', alignItems: 'center', marginBottom: 32 }}>
          <span style={{ fontSize: 56, fontWeight: 800, color: '#fafafa' }}>Career</span>
          <span style={{ fontSize: 56, fontWeight: 800, color: '#3b82f6' }}>Lens</span>
          <div style={{
            width: 14, height: 14, borderRadius: '50%',
            background: '#3b82f6', marginLeft: 6, marginBottom: 24,
          }} />
        </div>

        {/* Headline */}
        <div style={{ fontSize: 40, fontWeight: 700, color: '#fafafa', marginBottom: 16, lineHeight: 1.2 }}>
          AI ile Kariyerini Şekillendir
        </div>

        {/* Description */}
        <div style={{ fontSize: 24, color: '#a1a1aa', marginBottom: 48, maxWidth: 800 }}>
          CV analizi · Maaş karşılaştırma · Kişisel kariyer yol haritası
        </div>

        {/* Pills */}
        <div style={{ display: 'flex', gap: 16 }}>
          {[
            { label: '📄 CV Analizi', bg: '#1e3a5f', color: '#93c5fd' },
            { label: '💰 Maaş Karşılaştırma', bg: '#2d1b69', color: '#c4b5fd' },
            { label: '🗺️ Kariyer Roadmap', bg: '#064e3b', color: '#6ee7b7' },
          ].map((pill) => (
            <div key={pill.label} style={{
              padding: '12px 24px', borderRadius: 999,
              background: pill.bg, color: pill.color, fontSize: 20,
            }}>
              {pill.label}
            </div>
          ))}
        </div>

        {/* URL */}
        <div style={{ position: 'absolute', bottom: 60, right: 80, fontSize: 20, color: '#3b82f6' }}>
          careerlens-web.vercel.app
        </div>
      </div>
    ),
    { ...size }
  );
}
