import React, {createContext, ReactNode, useContext, useState} from 'react';

const defaultLanguage = 'csharp';

interface LanguageContextType {
    language: string;
    setLanguage: (language: string) => void;
}

const LanguageContext = createContext<LanguageContextType | undefined>(undefined);

interface LanguageProviderProps {
    children: ReactNode;
}

export const LanguageProvider = ({ children }: LanguageProviderProps) => {
    const [language, setLanguage] = useState<string>(defaultLanguage);

    return (
        <LanguageContext.Provider value={{ language, setLanguage }}>
            {children}
        </LanguageContext.Provider>
    );
};

export const useLanguage = (): LanguageContextType => {
    return useContext(LanguageContext);
};