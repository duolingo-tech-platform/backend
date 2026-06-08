using DuolingoTechPlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DuolingoTechPlatform.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Courses.AnyAsync()) return;

            // ── Cursos ──────────────────────────────────────────────
            var expoDeepDive = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Expo Deep Dive",
                Description = "Domine o desenvolvimento mobile com Expo e React Native do zero ao avançado."
            };
            var reactNativeAvancado = new Course
            {
                Id = Guid.NewGuid(),
                Title = "React Native Avançado",
                Description = "Técnicas avançadas de performance, hooks e arquitetura para apps React Native."
            };
            var awsParaApps = new Course
            {
                Id = Guid.NewGuid(),
                Title = "AWS para App Devs",
                Description = "Integre serviços AWS no seu app mobile: S3, Cognito, DynamoDB e muito mais."
            };

            context.Courses.AddRange(expoDeepDive, reactNativeAvancado, awsParaApps);

            // ── Módulos ─────────────────────────────────────────────
            var modExpo1 = new Module { Id = Guid.NewGuid(), CourseId = expoDeepDive.Id, Title = "Fundamentos do Expo", Order = 1 };
            var modExpo2 = new Module { Id = Guid.NewGuid(), CourseId = expoDeepDive.Id, Title = "Navegação e Roteamento", Order = 2 };

            var modRN1 = new Module { Id = Guid.NewGuid(), CourseId = reactNativeAvancado.Id, Title = "Hooks e Performance", Order = 1 };
            var modRN2 = new Module { Id = Guid.NewGuid(), CourseId = reactNativeAvancado.Id, Title = "Gerenciamento de Estado", Order = 2 };

            var modAWS1 = new Module { Id = Guid.NewGuid(), CourseId = awsParaApps.Id, Title = "AWS Fundamentos", Order = 1 };
            var modAWS2 = new Module { Id = Guid.NewGuid(), CourseId = awsParaApps.Id, Title = "Cognito e Autenticação", Order = 2 };

            context.Modules.AddRange(modExpo1, modExpo2, modRN1, modRN2, modAWS1, modAWS2);

            // ── Lições ──────────────────────────────────────────────
            var lesExpo1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "Introdução ao Expo", Content = "O que é Expo, suas vantagens e como configurar o ambiente de desenvolvimento.", Order = 1 };
            var lesExpo1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "Componentes Básicos", Content = "View, Text, Image, ScrollView e TouchableOpacity no React Native.", Order = 2 };
            var lesExpo1_3 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo1.Id, Title = "StyleSheet e Flexbox", Content = "Estilização com StyleSheet.create e layout com Flexbox.", Order = 3 };

            var lesExpo2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo2.Id, Title = "Expo Router", Content = "Roteamento baseado em arquivos com Expo Router v3.", Order = 1 };
            var lesExpo2_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modExpo2.Id, Title = "Stack e Tab Navigation", Content = "Navegação por pilha e abas com Expo Router.", Order = 2 };

            var lesRN1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN1.Id, Title = "useCallback e useMemo", Content = "Otimização de renders com memoização de funções e valores.", Order = 1 };
            var lesRN1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN1.Id, Title = "React.memo", Content = "Memoização de componentes para evitar re-renders desnecessários.", Order = 2 };

            var lesRN2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modRN2.Id, Title = "Context API", Content = "Gerenciamento de estado global com Context API e useContext.", Order = 1 };

            var lesAWS1_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS1.Id, Title = "IAM e Segurança", Content = "Identidade e controle de acesso com IAM na AWS.", Order = 1 };
            var lesAWS1_2 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS1.Id, Title = "Amazon S3 Básico", Content = "Upload e gerenciamento de arquivos com Amazon S3.", Order = 2 };

            var lesAWS2_1 = new Lesson { Id = Guid.NewGuid(), ModuleId = modAWS2.Id, Title = "Configurando Cognito", Content = "User Pools e autenticação com Amazon Cognito.", Order = 1 };

            context.Lessons.AddRange(
                lesExpo1_1, lesExpo1_2, lesExpo1_3,
                lesExpo2_1, lesExpo2_2,
                lesRN1_1, lesRN1_2, lesRN2_1,
                lesAWS1_1, lesAWS1_2, lesAWS2_1
            );

            // ── Exercícios ──────────────────────────────────────────
            // Lição: Introdução ao Expo
            var ex1 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "O que é o Expo?", CorrectAnswer = "Um framework open-source para criar apps React Native" };
            var ex1o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um framework open-source para criar apps React Native", IsCorrect = true };
            var ex1o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Uma linguagem de programação para iOS", IsCorrect = false };
            var ex1o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um banco de dados NoSQL mobile", IsCorrect = false };
            var ex1o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex1.Id, Text = "Um servidor de hospedagem para apps", IsCorrect = false };

            var ex2 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "Qual comando cria um novo projeto Expo?", CorrectAnswer = "npx create-expo-app" };
            var ex2o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "npx create-expo-app", IsCorrect = true };
            var ex2o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "npm init expo", IsCorrect = false };
            var ex2o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "expo start new", IsCorrect = false };
            var ex2o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex2.Id, Text = "yarn create expo-project", IsCorrect = false };

            var ex3 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_1.Id, Question = "O Expo permite desenvolvimento para Android e iOS com o mesmo código?", CorrectAnswer = "Verdadeiro" };
            var ex3o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex3.Id, Text = "Verdadeiro", IsCorrect = true };
            var ex3o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex3.Id, Text = "Falso", IsCorrect = false };

            // Lição: Componentes Básicos
            var ex4 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_2.Id, Question = "Qual componente React Native é usado para exibir texto?", CorrectAnswer = "Text" };
            var ex4o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Text", IsCorrect = true };
            var ex4o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Label", IsCorrect = false };
            var ex4o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Paragraph", IsCorrect = false };
            var ex4o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex4.Id, Text = "Span", IsCorrect = false };

            var ex5 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_2.Id, Question = "No React Native, a `div` do HTML é substituída por:", CorrectAnswer = "View" };
            var ex5o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "View", IsCorrect = true };
            var ex5o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Container", IsCorrect = false };
            var ex5o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Box", IsCorrect = false };
            var ex5o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex5.Id, Text = "Section", IsCorrect = false };

            // Lição: StyleSheet e Flexbox
            var ex6 = new Exercise { Id = Guid.NewGuid(), LessonId = lesExpo1_3.Id, Question = "Como se cria um StyleSheet no React Native?", CorrectAnswer = "StyleSheet.create({ ... })" };
            var ex6o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "StyleSheet.create({ ... })", IsCorrect = true };
            var ex6o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "new StyleSheet({ ... })", IsCorrect = false };
            var ex6o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "CSS.create({ ... })", IsCorrect = false };
            var ex6o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex6.Id, Text = "createStyle({ ... })", IsCorrect = false };

            // Lição: useCallback e useMemo
            var ex7 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_1.Id, Question = "O hook `useCallback` serve para memoizar:", CorrectAnswer = "Funções" };
            var ex7o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Funções", IsCorrect = true };
            var ex7o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Componentes inteiros", IsCorrect = false };
            var ex7o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Estados", IsCorrect = false };
            var ex7o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex7.Id, Text = "Contextos globais", IsCorrect = false };

            var ex8 = new Exercise { Id = Guid.NewGuid(), LessonId = lesRN1_1.Id, Question = "O `useMemo` recalcula o valor somente quando:", CorrectAnswer = "As dependências informadas mudam" };
            var ex8o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "As dependências informadas mudam", IsCorrect = true };
            var ex8o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "O componente renderiza sempre", IsCorrect = false };
            var ex8o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "Nunca — é calculado uma única vez", IsCorrect = false };
            var ex8o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex8.Id, Text = "Quando o estado global muda", IsCorrect = false };

            // Lição: IAM e Segurança
            var ex9 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_1.Id, Question = "IAM na AWS significa:", CorrectAnswer = "Identity and Access Management" };
            var ex9o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Identity and Access Management", IsCorrect = true };
            var ex9o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Internet Application Module", IsCorrect = false };
            var ex9o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Integrated Auth Middleware", IsCorrect = false };
            var ex9o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex9.Id, Text = "Internal Access Manager", IsCorrect = false };

            var ex10 = new Exercise { Id = Guid.NewGuid(), LessonId = lesAWS1_1.Id, Question = "No IAM, o que é uma Policy?", CorrectAnswer = "Um documento JSON que define permissões" };
            var ex10o1 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um documento JSON que define permissões", IsCorrect = true };
            var ex10o2 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um servidor de autenticação", IsCorrect = false };
            var ex10o3 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Um tipo de usuário administrador", IsCorrect = false };
            var ex10o4 = new ExerciseOption { Id = Guid.NewGuid(), ExerciseId = ex10.Id, Text = "Uma região geográfica da AWS", IsCorrect = false };

            context.Exercises.AddRange(ex1, ex2, ex3, ex4, ex5, ex6, ex7, ex8, ex9, ex10);
            context.ExerciseOptions.AddRange(
                ex1o1, ex1o2, ex1o3, ex1o4,
                ex2o1, ex2o2, ex2o3, ex2o4,
                ex3o1, ex3o2,
                ex4o1, ex4o2, ex4o3, ex4o4,
                ex5o1, ex5o2, ex5o3, ex5o4,
                ex6o1, ex6o2, ex6o3, ex6o4,
                ex7o1, ex7o2, ex7o3, ex7o4,
                ex8o1, ex8o2, ex8o3, ex8o4,
                ex9o1, ex9o2, ex9o3, ex9o4,
                ex10o1, ex10o2, ex10o3, ex10o4
            );

            await context.SaveChangesAsync();
        }
    }
}
