// components/ThemeSelector.tsx
import React, { useState, useEffect, useRef } from 'react';
import { Check, Palette, ChevronDown } from 'lucide-react';

// Define all DaisyUI theme types
type ThemeType = 
  | 'light' | 'dark' | 'cupcake' | 'bumblebee' | 'emerald' | 'corporate' 
  | 'synthwave' | 'retro' | 'cyberpunk' | 'valentine' | 'halloween' 
  | 'garden' | 'forest' | 'aqua' | 'lofi' | 'pastel' | 'fantasy' 
  | 'wireframe' | 'black' | 'luxury' | 'dracula' | 'cmyk' | 'autumn' 
  | 'business' | 'acid' | 'lemonade' | 'night' | 'coffee' | 'winter'
  | 'dim' | 'nord' | 'sunset' | 'caramellatte' | 'abyss' | 'silk';

interface Theme {
  id: ThemeType;
  name: string;
  icon: string;
}

// Complete list of all DaisyUI themes with their icons
const themeList: Theme[] = [
  { id: "light", name: "Light", icon: "☀️" },
  { id: "dark", name: "Dark", icon: "🌙" },
  { id: "cupcake", name: "Cupcake", icon: "🧁" },
  { id: "bumblebee", name: "Bumblebee", icon: "🐝" },
  { id: "emerald", name: "Emerald", icon: "✳️" },
  { id: "corporate", name: "Corporate", icon: "🏢" },
  { id: "synthwave", name: "Synthwave", icon: "🌆" },
  { id: "retro", name: "Retro", icon: "📺" },
  { id: "cyberpunk", name: "Cyberpunk", icon: "🤖" },
  { id: "valentine", name: "Valentine", icon: "❤️" },
  { id: "halloween", name: "Halloween", icon: "🎃" },
  { id: "garden", name: "Garden", icon: "🌷" },
  { id: "forest", name: "Forest", icon: "🌲" },
  { id: "aqua", name: "Aqua", icon: "🌊" },
  { id: "lofi", name: "Lo-Fi", icon: "📻" },
  { id: "pastel", name: "Pastel", icon: "🖌️" },
  { id: "fantasy", name: "Fantasy", icon: "🧙" },
  { id: "wireframe", name: "Wireframe", icon: "📝" },
  { id: "black", name: "Black", icon: "⚫" },
  { id: "luxury", name: "Luxury", icon: "💎" },
  { id: "dracula", name: "Dracula", icon: "🧛" },
  { id: "cmyk", name: "CMYK", icon: "🖨️" },
  { id: "autumn", name: "Autumn", icon: "🍂" },
  { id: "business", name: "Business", icon: "💼" },
  { id: "acid", name: "Acid", icon: "⚗️" },
  { id: "lemonade", name: "Lemonade", icon: "🍋" },
  { id: "night", name: "Night", icon: "🌃" },
  { id: "coffee", name: "Coffee", icon: "☕" },
  { id: "winter", name: "Winter", icon: "❄️" },
  { id: "dim", name: "Dim", icon: "🔅" },
  { id: "nord", name: "Nord", icon: "🧊" },
  { id: "sunset", name: "Sunset", icon: "🌅" },
  { id: "caramellatte", name: "Caramel Latte", icon: "🍮" },
  { id: "abyss", name: "Abyss", icon: "🌊" },
  { id: "silk", name: "Silk", icon: "🧣" }
];

interface ThemeSelectorProps {
  isExpanded?: boolean; // Si el sidebar está expandido
}

const ThemeSelector: React.FC<ThemeSelectorProps> = ({ isExpanded = true }) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [currentTheme, setCurrentTheme] = useState<ThemeType>('light');
  const menuRef = useRef<HTMLDivElement>(null);

  // Load saved theme on component mount
  useEffect(() => {
    // Get the current theme from the data-theme attribute
    const htmlTheme = document.documentElement.getAttribute('data-theme') as ThemeType | null;
    
    // Get theme from localStorage
    const savedTheme = localStorage.getItem('theme') as ThemeType | null;
    
    // Use the theme from HTML, localStorage, or default to 'light'
    const themeToUse = htmlTheme || savedTheme || 'light';
    setCurrentTheme(themeToUse);
    
    console.log('Current theme from HTML:', htmlTheme);
    console.log('Current theme from localStorage:', savedTheme);
    console.log('Using theme:', themeToUse);
  }, []);

  // Handle click outside to close menu
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Apply theme directly
  const applyTheme = (themeId: ThemeType) => {
    console.log('Applying theme:', themeId);
    
    try {
      // Set theme directly to HTML element
      document.documentElement.setAttribute('data-theme', themeId);
      
      // Also set on body for good measure
      document.body.setAttribute('data-theme', themeId);
      
      // Try setting on app root if it exists
      const appRoot = document.getElementById('root') || document.getElementById('app');
      if (appRoot) {
        appRoot.setAttribute('data-theme', themeId);
      }
      
      // Save to localStorage
      localStorage.setItem('theme', themeId);
      
      // Update component state
      setCurrentTheme(themeId);
      setIsMenuOpen(false);
      
      // Debug logging
      console.log('Theme applied successfully');
      console.log('HTML data-theme:', document.documentElement.getAttribute('data-theme'));
      console.log('localStorage theme:', localStorage.getItem('theme'));
    } catch (error) {
      console.error('Error applying theme:', error);
    }
  };

  // Get current theme details
  const getCurrentTheme = () => {
    return themeList.find(t => t.id === currentTheme) || themeList[0];
  };

  return (
    <div className="relative" ref={menuRef}>
      <div className="flex items-center justify-between">
        {isExpanded && <span className="text-sm">Theme</span>}
        <button 
          className={`btn btn-sm ${isExpanded ? 'btn-ghost' : 'btn-ghost btn-circle'}`} 
          onClick={() => setIsMenuOpen(!isMenuOpen)}
        >
          {isExpanded ? (
            <>
              <span className="mr-2">{getCurrentTheme().icon}</span>
              <span>{getCurrentTheme().name}</span>
            </>
          ) : (
            <Palette size={16} />
          )}
        </button>
      </div>
      
      {isMenuOpen && (
        <div className={`absolute ${isExpanded ? 'right-0' : 'left-1/2 -translate-x-1/2'} bottom-12 bg-base-100 rounded-lg shadow-lg p-2 z-50 w-48 border border-base-300`}>
          <div className="flex items-center justify-between px-3 py-2 border-b border-base-200 mb-2">
            <span className="font-medium">Select Theme</span>
            <button 
              className="btn btn-ghost btn-xs btn-circle"
              onClick={() => setIsMenuOpen(false)}
            >
              <ChevronDown size={14} />
            </button>
          </div>
          <div className="max-h-60 overflow-y-auto">
            {themeList.map((theme) => (
              <button 
                key={theme.id}
                className="flex items-center w-full px-3 py-2 hover:bg-base-200 rounded-md gap-2"
                onClick={() => applyTheme(theme.id)}
              >
                <span>{theme.icon}</span>
                <span>{theme.name}</span>
                {currentTheme === theme.id && <Check size={16} className="ml-auto text-primary" />}
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default ThemeSelector;